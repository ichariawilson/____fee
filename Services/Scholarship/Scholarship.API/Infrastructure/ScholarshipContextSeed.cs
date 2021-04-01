namespace Microsoft.Fee.Services.Scholarship.API.Infrastructure
{
    using Extensions.Logging;
    using global::Scholarship.API.Extensions;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Options;
    using Model;
    using Polly;
    using Polly.Retry;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class ScholarshipContextSeed
    {
        public async Task SeedAsync(ScholarshipContext context, IWebHostEnvironment env, IOptions<ScholarshipSettings> settings, ILogger<ScholarshipContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(ScholarshipContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                var useCustomizationData = settings.Value.UseCustomizationData;
                var contentRootPath = env.ContentRootPath;
                var picturePath = env.WebRootPath;

                if (!context.ScholarshipCurrencies.Any())
                {
                    await context.ScholarshipCurrencies.AddRangeAsync(useCustomizationData
                        ? GetScholarshipCurrenciesFromFile(contentRootPath, logger)
                        : GetPreconfiguredScholarshipCurrencies());

                    await context.SaveChangesAsync();
                }

                if (!context.ScholarshipDurations.Any())
                {
                    await context.ScholarshipDurations.AddRangeAsync(useCustomizationData
                        ? GetScholarshipDurationsFromFile(contentRootPath, logger)
                        : GetPreconfiguredScholarshipDurations());

                    await context.SaveChangesAsync();
                }

                if (!context.ScholarshipCourses.Any())
                {
                    await context.ScholarshipCourses.AddRangeAsync(useCustomizationData
                        ? GetScholarshipCoursesFromFile(contentRootPath, context, logger)
                        : GetPreconfiguredScholarshipCourses());

                    await context.SaveChangesAsync();
                }

                if (!context.ScholarshipEducationLevels.Any())
                {
                    await context.ScholarshipEducationLevels.AddRangeAsync(useCustomizationData
                        ? GetScholarshipEducationLevelsFromFile(contentRootPath, logger)
                        : GetPreconfiguredScholarshipEducationLevels());

                    await context.SaveChangesAsync();
                }

                if (!context.ScholarshipInterests.Any())
                {
                    await context.ScholarshipInterests.AddRangeAsync(useCustomizationData
                        ? GetScholarshipInterestsFromFile(contentRootPath, logger)
                        : GetPreconfiguredScholarshipInterests());

                    await context.SaveChangesAsync();
                }

                if (!context.ScholarshipLocations.Any())
                {
                    await context.ScholarshipLocations.AddRangeAsync(useCustomizationData
                        ? GetScholarshipLocationsFromFile(contentRootPath, logger)
                        : GetPreconfiguredScholarshipLocations());

                    await context.SaveChangesAsync();
                }

                if (!context.ScholarshipSchools.Any())
                {
                    await context.ScholarshipSchools.AddRangeAsync(useCustomizationData
                        ? GetScholarshipSchoolsFromFile(contentRootPath, context, logger)
                        : GetPreconfiguredScholarshipSchools());

                    await context.SaveChangesAsync();
                }

                if (!context.ScholarshipFeeStructures.Any())
                {
                    await context.ScholarshipFeeStructures.AddRangeAsync(useCustomizationData
                        ? GetScholarshipFeeStructuresFromFile(contentRootPath, context, logger)
                        : GetPreconfiguredScholarshipFeeStructures());

                    await context.SaveChangesAsync();
                }

                if (!context.ScholarshipItems.Any())
                {
                    await context.ScholarshipItems.AddRangeAsync(useCustomizationData
                        ? GetScholarshipItemsFromFile(contentRootPath, context, logger)
                        : GetPreconfiguredItems());

                    await context.SaveChangesAsync();

                    GetScholarshipItemPictures(contentRootPath, picturePath);
                }
            });
        }

        private IEnumerable<ScholarshipCourse> GetScholarshipCoursesFromFile(string contentRootPath, ScholarshipContext context, ILogger<ScholarshipContextSeed> logger)
        {
            string csvFileScholarshipCourses = Path.Combine(contentRootPath, "Setup", "ScholarshipCourses.csv");

            if (!File.Exists(csvFileScholarshipCourses))
            {
                return GetPreconfiguredScholarshipCourses();
            }

            string[] csvheaders;
            try
            {
                string[] requiredHeaders = { "name", "description", "fee", "duration" };
                csvheaders = GetHeaders(csvFileScholarshipCourses, requiredHeaders);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message);
                return GetPreconfiguredScholarshipCourses();
            }

            var scholarshipDurationIdLookup = context.ScholarshipDurations.ToDictionary(ct => ct.Duration, ct => ct.Id);

            return File.ReadAllLines(csvFileScholarshipCourses)
                        .Skip(1) // skip header row
                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                        .SelectTry(column => CreateScholarshipCourse(column, csvheaders, scholarshipDurationIdLookup))
                        .OnCaughtException(ex => { logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                        .Where(x => x != null);
        }

        private ScholarshipCourse CreateScholarshipCourse(string[] column, string[] headers, Dictionary<String, int> scholarshipDurationIdLookup)
        {
            if (column.Count() != headers.Count())
            {
                throw new Exception($"column count '{column.Count()}' not the same as headers count'{headers.Count()}'");
            }

            string scholarshipDurationName = column[Array.IndexOf(headers, "scholarshipduration")].Trim('"').Trim();
            if (!scholarshipDurationIdLookup.ContainsKey(scholarshipDurationName))
            {
                throw new Exception($"duration={scholarshipDurationName} does not exist in scholarshipDurations");
            }

            string feeString = column[Array.IndexOf(headers, "fee")].Trim('"').Trim();
            if (!Decimal.TryParse(feeString, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out Decimal fee))
            {
                throw new Exception($"fee={feeString}is not a valid decimal number");
            }

            var scholarshipCourse = new ScholarshipCourse()
            {
                ScholarshipDurationId = scholarshipDurationIdLookup[scholarshipDurationName],
                Description = column[Array.IndexOf(headers, "description")].Trim('"').Trim(),
                Fee = fee,
                Name = column[Array.IndexOf(headers, "name")].Trim('"').Trim(),
            };

            return scholarshipCourse;
        }

        private IEnumerable<ScholarshipCourse> GetPreconfiguredScholarshipCourses()
        {
            return new List<ScholarshipCourse>()
            {
                new ScholarshipCourse { Name = ".NET Bot", Description = "ersdyfku.jglh,iukfhcuviklhs/j,hmbjxhys", Fee = 80000.5M, ScholarshipDurationId = 1 },
                new ScholarshipCourse { Name = "Bot Black", Description = "ersdyfku.jglh,iukfhcuviklhs/j,hmbjxhys", Fee = 10000.5M, ScholarshipDurationId = 3 }
            };
        }

        private IEnumerable<ScholarshipCurrency> GetScholarshipCurrenciesFromFile(string contentRootPath, ILogger<ScholarshipContextSeed> logger)
        {
            string csvFileScholarshipCurrencies = Path.Combine(contentRootPath, "Setup", "ScholarshipCurrencies.csv");

            if (!File.Exists(csvFileScholarshipCurrencies))
            {
                return GetPreconfiguredScholarshipCurrencies();
            }

            string[] csvheaders;
            try
            {
                string[] requiredHeaders = { "symbol, code, currency" };
                csvheaders = GetHeaders(csvFileScholarshipCurrencies, requiredHeaders);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message);
                return GetPreconfiguredScholarshipCurrencies();
            }

            return File.ReadAllLines(csvFileScholarshipCurrencies)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(column => CreateScholarshipCurrency(column, csvheaders))
                                        .OnCaughtException(ex => { logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }

        private ScholarshipCurrency CreateScholarshipCurrency(string[] column, string[] headers)
        {
            if (column.Count() != headers.Count())
            {
                throw new Exception($"column count '{column.Count()}' not the same as headers count'{headers.Count()}'");
            }

            return new ScholarshipCurrency
            {
                Symbol = column[Array.IndexOf(headers, "symbol")].Trim('"').Trim(),
                Code = column[Array.IndexOf(headers, "code")].Trim('"').Trim(),
                Currency = column[Array.IndexOf(headers, "currency")].Trim('"').Trim()
            };
        }

        private IEnumerable<ScholarshipCurrency> GetPreconfiguredScholarshipCurrencies()
        {
            return new List<ScholarshipCurrency>()
            {
                new ScholarshipCurrency() { Symbol = "$", Code = "USD", Currency = "United States dollar"},
                new ScholarshipCurrency() { Symbol = "€", Code = "EUR", Currency = "Euro" },
                new ScholarshipCurrency() { Symbol = "£", Code = "GBP", Currency = "Pound sterling" },
                new ScholarshipCurrency() { Symbol = "¥", Code = "JPY", Currency = "Japanese yen" },
                new ScholarshipCurrency() { Symbol = "KSh", Code = "KES", Currency = "Kenyan shilling" }
            };
        }

        private IEnumerable<ScholarshipDuration> GetScholarshipDurationsFromFile(string contentRootPath, ILogger<ScholarshipContextSeed> logger)
        {
            string csvFileScholarshipDurations = Path.Combine(contentRootPath, "Setup", "ScholarshipDurations.csv");

            if (!File.Exists(csvFileScholarshipDurations))
            {
                return GetPreconfiguredScholarshipDurations();
            }

            string[] csvheaders;
            try
            {
                string[] requiredHeaders = { "scholarshipduration" };
                csvheaders = GetHeaders(csvFileScholarshipDurations, requiredHeaders);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message);
                return GetPreconfiguredScholarshipDurations();
            }

            return File.ReadAllLines(csvFileScholarshipDurations)
                                        .Skip(1) // skip header row
                                        .SelectTry(x => CreateScholarshipDuration(x))
                                        .OnCaughtException(ex => { logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }

        private ScholarshipDuration CreateScholarshipDuration(string duration)
        {
            duration = duration.Trim('"').Trim();

            if (String.IsNullOrEmpty(duration))
            {
                throw new Exception("Scholarship duration name is empty");
            }

            return new ScholarshipDuration
            {
                Duration = duration,
            };
        }

        private IEnumerable<ScholarshipDuration> GetPreconfiguredScholarshipDurations()
        {
            return new List<ScholarshipDuration>()
            {
                new ScholarshipDuration() { Duration = "One Term"},
                new ScholarshipDuration() { Duration = "One Semester" },
                new ScholarshipDuration() { Duration = "Two Terms" },
                new ScholarshipDuration() { Duration = "Two Semesters" }
            };
        }

        private IEnumerable<ScholarshipEducationLevel> GetScholarshipEducationLevelsFromFile(string contentRootPath, ILogger<ScholarshipContextSeed> logger)
        {
            string csvFileScholarshipEducationLevels = Path.Combine(contentRootPath, "Setup", "ScholarshipEducationLevels.csv");

            if (!File.Exists(csvFileScholarshipEducationLevels))
            {
                return GetPreconfiguredScholarshipEducationLevels();
            }

            string[] csvheaders;
            try
            {
                string[] requiredHeaders = { "educationLevel" };
                csvheaders = GetHeaders(csvFileScholarshipEducationLevels, requiredHeaders);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message);
                return GetPreconfiguredScholarshipEducationLevels();
            }

            return File.ReadAllLines(csvFileScholarshipEducationLevels)
                                        .Skip(1) // skip header row
                                        .SelectTry(x => CreateScholarshipEducationLevel(x))
                                        .OnCaughtException(ex => { logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }

        private ScholarshipEducationLevel CreateScholarshipEducationLevel(string educationLevel)
        {
            educationLevel = educationLevel.Trim('"').Trim();

            if (String.IsNullOrEmpty(educationLevel))
            {
                throw new Exception("Scholarship education level name is empty");
            }

            return new ScholarshipEducationLevel
            {
                EducationLevel = educationLevel,
            };
        }

        private IEnumerable<ScholarshipEducationLevel> GetPreconfiguredScholarshipEducationLevels()
        {
            return new List<ScholarshipEducationLevel>()
            {
                new ScholarshipEducationLevel() { EducationLevel = "College"},
                new ScholarshipEducationLevel() { EducationLevel = "Primary" },
                new ScholarshipEducationLevel() { EducationLevel = "Secondary" },
                new ScholarshipEducationLevel() { EducationLevel = "University" }
            };
        }

        private IEnumerable<ScholarshipFeeStructure> GetScholarshipFeeStructuresFromFile(string contentRootPath, ScholarshipContext context, ILogger<ScholarshipContextSeed> logger)
        {
            string csvFileScholarshipFeeStructures = Path.Combine(contentRootPath, "Setup", "ScholarshipFeeStructures.csv");

            if (!File.Exists(csvFileScholarshipFeeStructures))
            {
                return GetPreconfiguredScholarshipFeeStructures();
            }

            string[] csvheaders;
            try
            {
                string[] requiredHeaders = { "name", "fee", "course", "currency", "duration", "educationlevel", "school" };
                csvheaders = GetHeaders(csvFileScholarshipFeeStructures, requiredHeaders);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message);
                return GetPreconfiguredScholarshipFeeStructures();
            }

            var scholarshipCurrencyIdLookup = context.ScholarshipCurrencies.ToDictionary(ct => ct.Code, ct => ct.Id);
            var scholarshipDurationIdLookup = context.ScholarshipDurations.ToDictionary(ct => ct.Duration, ct => ct.Id);
            var scholarshipEducationLevelIdLookup = context.ScholarshipEducationLevels.ToDictionary(ct => ct.EducationLevel, ct => ct.Id);
            var scholarshipCourseIdLookup = context.ScholarshipCourses.ToDictionary(ct => ct.Name, ct => ct.Id);
            var scholarshipSchoolIdLookup = context.ScholarshipSchools.ToDictionary(ct => ct.Name, ct => ct.Id);

            return File.ReadAllLines(csvFileScholarshipFeeStructures)
                        .Skip(1) // skip header row
                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                        .SelectTry(column => CreateScholarshipFeeStructure(column, csvheaders, scholarshipCurrencyIdLookup, scholarshipDurationIdLookup, scholarshipEducationLevelIdLookup, scholarshipCourseIdLookup, scholarshipSchoolIdLookup))
                        .OnCaughtException(ex => { logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                        .Where(x => x != null);
        }

        private ScholarshipFeeStructure CreateScholarshipFeeStructure(string[] column, string[] headers, Dictionary<String, int> scholarshipCurrencyIdLookup, Dictionary<String, int> scholarshipDurationIdLookup, Dictionary<String, int> scholarshipEducationLevelIdLookup, Dictionary<String, int> scholarshipCourseIdLookup, Dictionary<String, int> scholarshipSchoolIdLookup)
        {
            if (column.Count() != headers.Count())
            {
                throw new Exception($"column count '{column.Count()}' not the same as headers count'{headers.Count()}'");
            }

            string scholarshipCurrencyName = column[Array.IndexOf(headers, "scholarshipcurrency")].Trim('"').Trim();
            if (!scholarshipCurrencyIdLookup.ContainsKey(scholarshipCurrencyName))
            {
                throw new Exception($"currency={scholarshipCurrencyName} does not exist in scholarshipCurrencys");
            }

            string scholarshipDurationName = column[Array.IndexOf(headers, "scholarshipduration")].Trim('"').Trim();
            if (!scholarshipDurationIdLookup.ContainsKey(scholarshipDurationName))
            {
                throw new Exception($"duration={scholarshipDurationName} does not exist in scholarshipDurations");
            }

            string scholarshipEducationLevelName = column[Array.IndexOf(headers, "scholarshipeducationlevel")].Trim('"').Trim();
            if (!scholarshipEducationLevelIdLookup.ContainsKey(scholarshipEducationLevelName))
            {
                throw new Exception($"educationlevel={scholarshipEducationLevelName} does not exist in scholarshipEducationLevels");
            }

            string scholarshipCourseName = column[Array.IndexOf(headers, "scholarshipcourse")].Trim('"').Trim();
            if (!scholarshipCourseIdLookup.ContainsKey(scholarshipCourseName))
            {
                throw new Exception($"course={scholarshipCourseName} does not exist in scholarshipCourses");
            }

            string scholarshipSchoolName = column[Array.IndexOf(headers, "scholarshipschool")].Trim('"').Trim();
            if (!scholarshipSchoolIdLookup.ContainsKey(scholarshipSchoolName))
            {
                throw new Exception($"school={scholarshipSchoolName} does not exist in scholarshipSchools");
            }

            string feeString = column[Array.IndexOf(headers, "fee")].Trim('"').Trim();
            if (!Decimal.TryParse(feeString, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out Decimal fee))
            {
                throw new Exception($"fee={feeString}is not a valid decimal number");
            }

            var scholarshipFeeStructure = new ScholarshipFeeStructure()
            {
                ScholarshipCurrencyId = scholarshipCurrencyIdLookup[scholarshipCurrencyName],
                ScholarshipDurationId = scholarshipDurationIdLookup[scholarshipDurationName],
                ScholarshipEducationLevelId = scholarshipEducationLevelIdLookup[scholarshipEducationLevelName],
                ScholarshipCourseId = scholarshipCourseIdLookup[scholarshipCourseName],
                ScholarshipSchoolId = scholarshipSchoolIdLookup[scholarshipSchoolName],
                Fee = fee,
                Name = column[Array.IndexOf(headers, "name")].Trim('"').Trim(),
            };

            return scholarshipFeeStructure;
        }

        private IEnumerable<ScholarshipFeeStructure> GetPreconfiguredScholarshipFeeStructures()
        {
            return new List<ScholarshipFeeStructure>()
            {
                new ScholarshipFeeStructure { ScholarshipCourseId = 1, ScholarshipDurationId = 1, ScholarshipEducationLevelId = 1, ScholarshipCurrencyId = 1, ScholarshipSchoolId = 1, Name = ".NET Bot Black Hoodie", Fee = 80000.5M },
            };
        }

        private IEnumerable<ScholarshipInterest> GetScholarshipInterestsFromFile(string contentRootPath, ILogger<ScholarshipContextSeed> logger)
        {
            string csvFileScholarshipInterests = Path.Combine(contentRootPath, "Setup", "ScholarshipInterests.csv");

            if (!File.Exists(csvFileScholarshipInterests))
            {
                return GetPreconfiguredScholarshipInterests();
            }

            string[] csvheaders;
            try
            {
                string[] requiredHeaders = { "interest" };
                csvheaders = GetHeaders(csvFileScholarshipInterests, requiredHeaders);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message);
                return GetPreconfiguredScholarshipInterests();
            }

            return File.ReadAllLines(csvFileScholarshipInterests)
                                        .Skip(1) // skip header row
                                        .SelectTry(x => CreateScholarshipInterest(x))
                                        .OnCaughtException(ex => { logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }

        private ScholarshipInterest CreateScholarshipInterest(string interest)
        {
            interest = interest.Trim('"').Trim();

            if (String.IsNullOrEmpty(interest))
            {
                throw new Exception("Scholarship interest name is empty");
            }

            return new ScholarshipInterest
            {
                Interest = interest,
            };
        }

        private IEnumerable<ScholarshipInterest> GetPreconfiguredScholarshipInterests()
        {
            return new List<ScholarshipInterest>()
            {
                new ScholarshipInterest() { Interest = "Music"},
                new ScholarshipInterest() { Interest = "Farming" },
                new ScholarshipInterest() { Interest = "Carpentry" },
                new ScholarshipInterest() { Interest = "Food" }
            };
        }

        private IEnumerable<ScholarshipLocation> GetScholarshipLocationsFromFile(string contentRootPath, ILogger<ScholarshipContextSeed> logger)
        {
            string csvFileScholarshipLocations = Path.Combine(contentRootPath, "Setup", "ScholarshipLocations.csv");

            if (!File.Exists(csvFileScholarshipLocations))
            {
                return GetPreconfiguredScholarshipLocations();
            }

            string[] csvheaders;
            try
            {
                string[] requiredHeaders = { "location" };
                csvheaders = GetHeaders(csvFileScholarshipLocations, requiredHeaders);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message);
                return GetPreconfiguredScholarshipLocations();
            }

            return File.ReadAllLines(csvFileScholarshipLocations)
                                        .Skip(1) // skip header row
                                        .SelectTry(x => CreateScholarshipLocation(x))
                                        .OnCaughtException(ex => { logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }

        private ScholarshipLocation CreateScholarshipLocation(string educationLevel)
        {
            educationLevel = educationLevel.Trim('"').Trim();

            if (String.IsNullOrEmpty(educationLevel))
            {
                throw new Exception("Scholarship location name is empty");
            }

            return new ScholarshipLocation
            {
                Location = educationLevel,
            };
        }

        private IEnumerable<ScholarshipLocation> GetPreconfiguredScholarshipLocations()
        {
            return new List<ScholarshipLocation>()
            {
                new ScholarshipLocation() { Location = "Makueni"},
                new ScholarshipLocation() { Location = "Busia" },
                new ScholarshipLocation() { Location = "Narok" },
                new ScholarshipLocation() { Location = "Laikipia" },
                new ScholarshipLocation() { Location = "Tana River" },
                new ScholarshipLocation() { Location = "Siaya" },
                new ScholarshipLocation() { Location = "Marsabit" },
                new ScholarshipLocation() { Location = "Nairobi" },
                new ScholarshipLocation() { Location = "Kwale" }
            };
        }

        private IEnumerable<ScholarshipItem> GetScholarshipItemsFromFile(string contentRootPath, ScholarshipContext context, ILogger<ScholarshipContextSeed> logger)
        {
            string csvFileScholarshipItems = Path.Combine(contentRootPath, "Setup", "ScholarshipItems.csv");

            if (!File.Exists(csvFileScholarshipItems))
            {
                return GetPreconfiguredItems();
            }

            string[] csvheaders;
            try
            {
                string[] requiredHeaders = { "currencysymbol", "duration", "educationlevel", "interest", "location", "description", "name", "amount", "picturefilename" };
                string[] optionalheaders = { "availableslots", "reslotthreshold", "maxslotthreshold", "onreapply" };
                csvheaders = GetHeaders(csvFileScholarshipItems, requiredHeaders, optionalheaders);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message);
                return GetPreconfiguredItems();
            }

            var scholarshipCurrencyIdLookup = context.ScholarshipCurrencies.ToDictionary(ct => ct.Currency, ct => ct.Id);
            var scholarshipDurationIdLookup = context.ScholarshipDurations.ToDictionary(ct => ct.Duration, ct => ct.Id);
            var scholarshipEducationlevelIdLookup = context.ScholarshipEducationLevels.ToDictionary(ct => ct.EducationLevel, ct => ct.Id);
            var scholarshipInterestIdLookup = context.ScholarshipInterests.ToDictionary(ct => ct.Interest, ct => ct.Id);
            var scholarshipLocationIdLookup = context.ScholarshipLocations.ToDictionary(ct => ct.Location, ct => ct.Id);

            return File.ReadAllLines(csvFileScholarshipItems)
                        .Skip(1) // skip header row
                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                        .SelectTry(column => CreateScholarshipItem(column, csvheaders, scholarshipCurrencyIdLookup, scholarshipDurationIdLookup, scholarshipEducationlevelIdLookup, scholarshipInterestIdLookup, scholarshipLocationIdLookup))
                        .OnCaughtException(ex => { logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                        .Where(x => x != null);
        }

        private ScholarshipItem CreateScholarshipItem(string[] column, string[] headers, Dictionary<String, int> scholarshipCurrencyIdLookup, Dictionary<String, int> scholarshipDurationIdLookup, Dictionary<String, int> scholarshipEducationlevelIdLookup, Dictionary<String, int> scholarshipInterestIdLookup, Dictionary<String, int> scholarshipLocationIdLookup)
        {
            if (column.Count() != headers.Count())
            {
                throw new Exception($"column count '{column.Count()}' not the same as headers count'{headers.Count()}'");
            }

            string scholarshipCurrencyName = column[Array.IndexOf(headers, "scholarshipcurrency")].Trim('"').Trim();
            if (!scholarshipCurrencyIdLookup.ContainsKey(scholarshipCurrencyName))
            {
                throw new Exception($"currency={scholarshipCurrencyName} does not exist in scholarshipCurrencies");
            }

            string scholarshipDurationName = column[Array.IndexOf(headers, "scholarshipduration")].Trim('"').Trim();
            if (!scholarshipDurationIdLookup.ContainsKey(scholarshipDurationName))
            {
                throw new Exception($"duration={scholarshipDurationName} does not exist in scholarshipDurations");
            }

            string scholarshipEducationlevelName = column[Array.IndexOf(headers, "scholarshipeducationlevel")].Trim('"').Trim();
            if (!scholarshipEducationlevelIdLookup.ContainsKey(scholarshipEducationlevelName))
            {
                throw new Exception($"educationlevel={scholarshipEducationlevelName} does not exist in scholarshipEducationlevels");
            }

            string scholarshipInterestName = column[Array.IndexOf(headers, "scholarshipinterest")].Trim('"').Trim();
            if (!scholarshipInterestIdLookup.ContainsKey(scholarshipInterestName))
            {
                throw new Exception($"interest={scholarshipInterestName} does not exist in scholarshipDurations");
            }

            string scholarshipLocationName = column[Array.IndexOf(headers, "scholarshiplocation")].Trim('"').Trim();
            if (!scholarshipLocationIdLookup.ContainsKey(scholarshipLocationName))
            {
                throw new Exception($"location={scholarshipLocationName} does not exist in scholarshipLocations");
            }

            string amountString = column[Array.IndexOf(headers, "amount")].Trim('"').Trim();
            if (!Decimal.TryParse(amountString, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out Decimal amount))
            {
                throw new Exception($"amount={amountString}is not a valid decimal number");
            }

            var scholarshipItem = new ScholarshipItem()
            {
                ScholarshipCurrencyId = scholarshipCurrencyIdLookup[scholarshipCurrencyName],
                ScholarshipDurationId = scholarshipDurationIdLookup[scholarshipDurationName],
                ScholarshipEducationLevelId = scholarshipEducationlevelIdLookup[scholarshipEducationlevelName],
                ScholarshipInterestId = scholarshipInterestIdLookup[scholarshipInterestName],
                ScholarshipLocationId = scholarshipLocationIdLookup[scholarshipLocationName],
                Description = column[Array.IndexOf(headers, "description")].Trim('"').Trim(),
                Name = column[Array.IndexOf(headers, "name")].Trim('"').Trim(),
                Amount = amount,
                PictureFileName = column[Array.IndexOf(headers, "picturefilename")].Trim('"').Trim(),
            };

            int availableSlotsIndex = Array.IndexOf(headers, "availableslots");
            if (availableSlotsIndex != -1)
            {
                string availableSlotsString = column[availableSlotsIndex].Trim('"').Trim();
                if (!String.IsNullOrEmpty(availableSlotsString))
                {
                    if (int.TryParse(availableSlotsString, out int availableSlots))
                    {
                        scholarshipItem.AvailableSlots = availableSlots;
                    }
                    else
                    {
                        throw new Exception($"availableSlots={availableSlotsString} is not a valid integer");
                    }
                }
            }

            int reslotThresholdIndex = Array.IndexOf(headers, "reslotthreshold");
            if (reslotThresholdIndex != -1)
            {
                string reslotThresholdString = column[reslotThresholdIndex].Trim('"').Trim();
                if (!String.IsNullOrEmpty(reslotThresholdString))
                {
                    if (int.TryParse(reslotThresholdString, out int reslotThreshold))
                    {
                        scholarshipItem.ReslotThreshold = reslotThreshold;
                    }
                    else
                    {
                        throw new Exception($"reslotThreshold={reslotThreshold} is not a valid integer");
                    }
                }
            }

            int maxSlotThresholdIndex = Array.IndexOf(headers, "maxslotthreshold");
            if (maxSlotThresholdIndex != -1)
            {
                string maxSlotThresholdString = column[maxSlotThresholdIndex].Trim('"').Trim();
                if (!String.IsNullOrEmpty(maxSlotThresholdString))
                {
                    if (int.TryParse(maxSlotThresholdString, out int maxSlotThreshold))
                    {
                        scholarshipItem.MaxSlotThreshold = maxSlotThreshold;
                    }
                    else
                    {
                        throw new Exception($"maxSlotThreshold={maxSlotThreshold} is not a valid integer");
                    }
                }
            }

            int onReapplyIndex = Array.IndexOf(headers, "onreapply");
            if (onReapplyIndex != -1)
            {
                string onReapplyString = column[onReapplyIndex].Trim('"').Trim();
                if (!String.IsNullOrEmpty(onReapplyString))
                {
                    if (bool.TryParse(onReapplyString, out bool onReapply))
                    {
                        scholarshipItem.OnReapply = onReapply;
                    }
                    else
                    {
                        throw new Exception($"onReapply={onReapplyString} is not a valid boolean");
                    }
                }
            }

            return scholarshipItem;
        }

        private IEnumerable<ScholarshipItem> GetPreconfiguredItems()
        {
            return new List<ScholarshipItem>()
            {
                new ScholarshipItem { ScholarshipDurationId = 2, ScholarshipCurrencyId = 2, ScholarshipEducationLevelId = 2, ScholarshipInterestId = 1, ScholarshipLocationId = 1, AvailableSlots = 100, Description = "Leonardo Bigollo Pisano", Name = "1st Scholarship", Amount = 19000M, PictureFileName = "1.png" },
                new ScholarshipItem { ScholarshipDurationId = 1, ScholarshipCurrencyId = 2, ScholarshipEducationLevelId = 2, ScholarshipInterestId = 2, ScholarshipLocationId = 2, AvailableSlots = 100, Description = "Leonardo Bigollo Pisano", Name = "2nd Scholarship", Amount= 8500M, PictureFileName = "2.png" },
                new ScholarshipItem { ScholarshipDurationId = 2, ScholarshipCurrencyId = 5, ScholarshipEducationLevelId = 1, ScholarshipInterestId = 4, ScholarshipLocationId = 3, AvailableSlots = 100, Description = "Leonardo Bigollo Pisano", Name = "3rd Scholarship", Amount = 12000, PictureFileName = "3.png" },
                new ScholarshipItem { ScholarshipDurationId = 2, ScholarshipCurrencyId = 2, ScholarshipEducationLevelId = 3, ScholarshipInterestId = 4, ScholarshipLocationId = 4, AvailableSlots = 100, Description = "Leonardo Bigollo Pisano", Name = "5th Scholarship", Amount = 10200, PictureFileName = "4.png" },
                new ScholarshipItem { ScholarshipDurationId = 3, ScholarshipCurrencyId = 5, ScholarshipEducationLevelId = 3, ScholarshipInterestId = 1, ScholarshipLocationId = 5, AvailableSlots = 100, Description = "Leonardo Bigollo Pisano", Name = "8th Scholarship", Amount = 80000M, PictureFileName = "5.png" },
                new ScholarshipItem { ScholarshipDurationId = 2, ScholarshipCurrencyId = 2, ScholarshipEducationLevelId = 1, ScholarshipInterestId = 2, ScholarshipLocationId = 6, AvailableSlots = 100, Description = "Leonardo Bigollo Pisano", Name = "13th Scholarship", Amount = 1300, PictureFileName = "6.png" },
                new ScholarshipItem { ScholarshipDurationId = 2, ScholarshipCurrencyId = 5, ScholarshipEducationLevelId = 2, ScholarshipInterestId = 3, ScholarshipLocationId = 7, AvailableSlots = 100, Description = "Leonardo Bigollo Pisano", Name = "21th Scholarship", Amount = 21000, PictureFileName = "7.png" },
                new ScholarshipItem { ScholarshipDurationId = 2, ScholarshipCurrencyId = 5, ScholarshipEducationLevelId = 2, ScholarshipInterestId = 3, ScholarshipLocationId = 8, AvailableSlots = 100, Description = "Leonardo Bigollo Pisano", Name = "34th Scholarship", Amount = 34000M, PictureFileName = "8.png" },
                new ScholarshipItem { ScholarshipDurationId = 1, ScholarshipCurrencyId = 5, ScholarshipEducationLevelId = 2, ScholarshipInterestId = 2, ScholarshipLocationId = 9, AvailableSlots = 100, Description = "Leonardo Bigollo Pisano", Name = "55th Scholarship", Amount = 5500, PictureFileName = "9.png" },
                new ScholarshipItem { ScholarshipDurationId = 3, ScholarshipCurrencyId = 2, ScholarshipEducationLevelId = 4, ScholarshipInterestId = 1, ScholarshipLocationId = 4, AvailableSlots = 100, Description = "Leonardo Bigollo Pisano", Name = "89th Scholarship", Amount = 89M, PictureFileName = "10.png" },
                new ScholarshipItem { ScholarshipDurationId = 3, ScholarshipCurrencyId = 2, ScholarshipEducationLevelId = 4, ScholarshipInterestId = 1, ScholarshipLocationId = 2, AvailableSlots = 100, Description = "Leonardo Bigollo Pisano", Name = "144th Scholarship", Amount = 14400M, PictureFileName = "11.png" },
                new ScholarshipItem { ScholarshipDurationId = 2, ScholarshipCurrencyId = 5, ScholarshipEducationLevelId = 4, ScholarshipInterestId = 2, ScholarshipLocationId = 8, AvailableSlots = 100, Description = "Leonardo Bigollo Pisano", Name = "233rd Scholarship", Amount = 23300M, PictureFileName = "12.png" },
            };
        }

        private IEnumerable<ScholarshipSchool> GetScholarshipSchoolsFromFile(string contentRootPath, ScholarshipContext context, ILogger<ScholarshipContextSeed> logger)
        {
            string csvFileScholarshipSchools = Path.Combine(contentRootPath, "Setup", "ScholarshipSchools.csv");

            if (!File.Exists(csvFileScholarshipSchools))
            {
                return GetPreconfiguredScholarshipSchools();
            }

            string[] csvheaders;
            try
            {
                string[] requiredHeaders = { "scholarshipschool", "name", "phonenumber", "emailaddress" };
                csvheaders = GetHeaders(csvFileScholarshipSchools, requiredHeaders);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message);
                return GetPreconfiguredScholarshipSchools();
            }

            var scholarshipLocationIdLookup = context.ScholarshipLocations.ToDictionary(ct => ct.Location, ct => ct.Id);

            return File.ReadAllLines(csvFileScholarshipSchools)
                        .Skip(1) // skip header row
                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                        .SelectTry(column => CreateScholarshipSchool(column, csvheaders, scholarshipLocationIdLookup))
                        .OnCaughtException(ex => { logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                        .Where(x => x != null);
        }

        private ScholarshipSchool CreateScholarshipSchool(string[] column, string[] headers, Dictionary<String, int> scholarshipLocationIdLookup)
        {
            if (column.Count() != headers.Count())
            {
                throw new Exception($"column count '{column.Count()}' not the same as headers count'{headers.Count()}'");
            }

            string scholarshipLocationName = column[Array.IndexOf(headers, "scholarshiplocation")].Trim('"').Trim();
            if (!scholarshipLocationIdLookup.ContainsKey(scholarshipLocationName))
            {
                throw new Exception($"location={scholarshipLocationName} does not exist in scholarshipLocations");
            }

            var scholarshipSchool = new ScholarshipSchool()
            {
                ScholarshipLocationId = scholarshipLocationIdLookup[scholarshipLocationName],
                EmailAddress = column[Array.IndexOf(headers, "emailaddress")].Trim('"').Trim(),
                PhoneNumber = column[Array.IndexOf(headers, "phonenumber")].Trim('"').Trim(),
                Name = column[Array.IndexOf(headers, "name")].Trim('"').Trim(),
            };

            return scholarshipSchool;
        }

        private IEnumerable<ScholarshipSchool> GetPreconfiguredScholarshipSchools()
        {
            return new List<ScholarshipSchool>()
            {
                new ScholarshipSchool { ScholarshipLocationId = 1, Name = ".NET Bot Black Hoodie", EmailAddress = "1.png", PhoneNumber = "" },
            };
        }

        private string[] GetHeaders(string csvfile, string[] requiredHeaders, string[] optionalHeaders = null)
        {
            string[] csvheaders = File.ReadLines(csvfile).First().ToLowerInvariant().Split(',');

            if (csvheaders.Count() < requiredHeaders.Count())
            {
                throw new Exception($"requiredHeader count '{ requiredHeaders.Count()}' is bigger then csv header count '{csvheaders.Count()}' ");
            }

            if (optionalHeaders != null)
            {
                if (csvheaders.Count() > (requiredHeaders.Count() + optionalHeaders.Count()))
                {
                    throw new Exception($"csv header count '{csvheaders.Count()}'  is larger then required '{requiredHeaders.Count()}' and optional '{optionalHeaders.Count()}' headers count");
                }
            }

            foreach (var requiredHeader in requiredHeaders)
            {
                if (!csvheaders.Contains(requiredHeader))
                {
                    throw new Exception($"does not contain required header '{requiredHeader}'");
                }
            }

            return csvheaders;
        }

        private void GetScholarshipItemPictures(string contentRootPath, string picturePath)
        {
            if (picturePath != null)
            {
                DirectoryInfo directory = new DirectoryInfo(picturePath);
                foreach (FileInfo file in directory.GetFiles())
                {
                    file.Delete();
                }

                string zipFileScholarshipItemPictures = Path.Combine(contentRootPath, "Setup", "ScholarshipItems.zip");
                ZipFile.ExtractToDirectory(zipFileScholarshipItemPictures, picturePath);
            }
        }

        private AsyncRetryPolicy CreatePolicy(ILogger<ScholarshipContextSeed> logger, string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", prefix, exception.GetType().Name, exception.Message, retry, retries);
                    }
                );
        }
    }
}