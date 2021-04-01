namespace Microsoft.Fee.Services.Applying.API.Infrastructure
{
    using global::Applying.API.Extensions;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Fee.Services.Applying.Domain.AggregatesModel.ApplicationAggregate;
    using Microsoft.Fee.Services.Applying.Domain.AggregatesModel.StudentAggregate;
    using Microsoft.Fee.Services.Applying.Domain.SeedWork;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Applying.Infrastructure;
    using Polly;
    using Polly.Retry;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class ApplyingContextSeed
    {
        public async Task SeedAsync(ApplyingContext context, IWebHostEnvironment env, IOptions<ApplyingSettings> settings, ILogger<ApplyingContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(ApplyingContextSeed));

            await policy.ExecuteAsync(async () =>
            {

                var useCustomizationData = settings.Value
                .UseCustomizationData;

                var contentRootPath = env.ContentRootPath;


                using (context)
                {
                    context.Database.Migrate();

                    if (!context.PaymentTypes.Any())
                    {
                        context.PaymentTypes.AddRange(useCustomizationData
                                                ? GetPaymentTypesFromFile(contentRootPath, logger)
                                                : GetPredefinedPaymentTypes());

                        await context.SaveChangesAsync();
                    }

                    if (!context.ApplicationStatus.Any())
                    {
                        context.ApplicationStatus.AddRange(useCustomizationData
                                                ? GetApplicationStatusFromFile(contentRootPath, logger)
                                                : GetPredefinedApplicationStatus());
                    }

                    await context.SaveChangesAsync();
                }
            });
        }

        private IEnumerable<PaymentType> GetPaymentTypesFromFile(string contentRootPath, ILogger<ApplyingContextSeed> log)
        {
            string csvFilePaymentTypes = Path.Combine(contentRootPath, "Setup", "PaymentTypes.csv");

            if (!File.Exists(csvFilePaymentTypes))
            {
                return GetPredefinedPaymentTypes();
            }

            string[] csvheaders;
            try
            {
                string[] requiredHeaders = { "PaymentType" };
                csvheaders = GetHeaders(requiredHeaders, csvFilePaymentTypes);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message);
                return GetPredefinedPaymentTypes();
            }

            int id = 1;
            return File.ReadAllLines(csvFilePaymentTypes)
                                        .Skip(1) // skip header column
                                        .SelectTry(x => CreatePaymentType(x, ref id))
                                        .OnCaughtException(ex => { log.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }

        private PaymentType CreatePaymentType(string value, ref int id)
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new Exception("Applicationstatus is null or empty");
            }

            return new PaymentType(id++, value.Trim('"').Trim());
        }

        private IEnumerable<PaymentType> GetPredefinedPaymentTypes()
        {
            return Enumeration.GetAll<PaymentType>();
        }

        private IEnumerable<ApplicationStatus> GetApplicationStatusFromFile(string contentRootPath, ILogger<ApplyingContextSeed> log)
        {
            string csvFileApplicationStatus = Path.Combine(contentRootPath, "Setup", "ApplicationStatus.csv");

            if (!File.Exists(csvFileApplicationStatus))
            {
                return GetPredefinedApplicationStatus();
            }

            string[] csvheaders;
            try
            {
                string[] requiredHeaders = { "ApplicationStatus" };
                csvheaders = GetHeaders(requiredHeaders, csvFileApplicationStatus);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message);
                return GetPredefinedApplicationStatus();
            }

            int id = 1;
            return File.ReadAllLines(csvFileApplicationStatus)
                                        .Skip(1) // skip header row
                                        .SelectTry(x => CreateApplicationStatus(x, ref id))
                                        .OnCaughtException(ex => { log.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }

        private ApplicationStatus CreateApplicationStatus(string value, ref int id)
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new Exception("Applicationstatus is null or empty");
            }

            return new ApplicationStatus(id++, value.Trim('"').Trim().ToLowerInvariant());
        }

        private IEnumerable<ApplicationStatus> GetPredefinedApplicationStatus()
        {
            return new List<ApplicationStatus>()
            {
                ApplicationStatus.Submitted,
                ApplicationStatus.AwaitingValidation,
                ApplicationStatus.SlotConfirmed,
                ApplicationStatus.Paid,
                ApplicationStatus.Granted,
                ApplicationStatus.Cancelled
            };
        }

        private string[] GetHeaders(string[] requiredHeaders, string csvfile)
        {
            string[] csvheaders = File.ReadLines(csvfile).First().ToLowerInvariant().Split(',');

            if (csvheaders.Count() != requiredHeaders.Count())
            {
                throw new Exception($"requiredHeader count '{ requiredHeaders.Count()}' is different then read header '{csvheaders.Count()}'");
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


        private AsyncRetryPolicy CreatePolicy(ILogger<ApplyingContextSeed> logger, string prefix, int retries = 3)
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
