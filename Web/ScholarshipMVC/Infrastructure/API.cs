namespace ScholarshipMVC.Infrastructure
{
    public static class API
    {
        public static class Application
        {
            public static string GetApplication(string baseUri, string applicationId)
            {
                return $"{baseUri}/{applicationId}";
            }

            public static string GetAllMyApplications(string baseUri)
            {
                return baseUri;
            }

            public static string AddNewApplication(string baseUri)
            {
                return $"{baseUri}/new";
            }

            public static string CancelApplication(string baseUri)
            {
                return $"{baseUri}/cancel";
            }

            public static string GrantApplication(string baseUri)
            {
                return $"{baseUri}/grant";
            }
        }

        public static class Scholarship
        {
            public static string GetAllScholarshipItems(string baseUri, int page, int take, int? edlevel, int? location)
            {
                var filterQs = "";

                if (location.HasValue)
                {
                    var edlevelQs = (edlevel.HasValue) ? edlevel.Value.ToString() : string.Empty;
                    filterQs = $"/location/{location.Value}/edlevel/{edlevelQs}";

                }
                else if (edlevel.HasValue)
                {
                    var edlevelQs = (edlevel.HasValue) ? edlevel.Value.ToString() : string.Empty;
                    filterQs = $"/location/all/edlevel/{edlevelQs}";
                }
                else
                {
                    filterQs = string.Empty;
                }

                return $"{baseUri}items{filterQs}?pageIndex={page}&pageSize={take}";
            }

            public static string GetAllEducationLevels(string baseUri)
            {
                return $"{baseUri}scholarshipEducationLevels";
            }

            public static string GetAllLocations(string baseUri)
            {
                return $"{baseUri}scholarshipLocations";
            }

            public static string GetAllDurations(string baseUri)
            {
                return $"{baseUri}scholarshipDurations";
            }

            public static string GetAllInterests(string baseUri)
            {
                return $"{baseUri}scholarshipInterests";
            }

            public static string GetAllCurrencies(string baseUri)
            {
                return $"{baseUri}scholarshipCurrencies";
            }
        }
    }
}