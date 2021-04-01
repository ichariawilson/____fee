namespace WebMVC.Infrastructure
{
    public static class API
    {

        public static class Apply
        {
            public static string AddItemToBasket(string baseUri) => $"{baseUri}/basket/items";
            public static string UpdateBasketItem(string baseUri) => $"{baseUri}/basket/items";

            public static string GetApplicationDraft(string baseUri, string basketId) => $"{baseUri}/application/draft/{basketId}";
        }

        public static class Basket
        {
            public static string GetBasket(string baseUri, string basketId) => $"{baseUri}/{basketId}";
            public static string UpdateBasket(string baseUri) => baseUri;
            public static string CheckoutBasket(string baseUri) => $"{baseUri}/checkout";
            public static string CleanBasket(string baseUri, string basketId) => $"{baseUri}/{basketId}";
        }

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
            public static string GetAllScholarshipItems(string baseUri, int page, int take, int? location, int? educationlevel)
            {
                var filterQs = "";

                if (educationlevel.HasValue)
                {
                    var locationQs = (location.HasValue) ? location.Value.ToString() : string.Empty;
                    filterQs = $"/educationlevel/{educationlevel.Value}/location/{locationQs}";

                }
                else if (location.HasValue)
                {
                    var locationQs = (location.HasValue) ? location.Value.ToString() : string.Empty;
                    filterQs = $"/educationlevel/all/location/{locationQs}";
                }
                else
                {
                    filterQs = string.Empty;
                }

                return $"{baseUri}items{filterQs}?pageIndex={page}&pageSize={take}";
            }

            public static string GetAllCurrencies(string baseUri)
            {
                return $"{baseUri}scholarshipCurrencies";
            }

            public static string GetAllDurations(string baseUri)
            {
                return $"{baseUri}scholarshipDurations";
            }

            public static string GetAllEducationLevels(string baseUri)
            {
                return $"{baseUri}scholarshipEducationLevels";
            }

            public static string GetAllFeeStructures(string baseUri)
            {
                return $"{baseUri}scholarshipFeeStructures";
            }

            public static string GetAllInterests(string baseUri)
            {
                return $"{baseUri}scholarshipInterests";
            }

            public static string GetAllLocations(string baseUri)
            {
                return $"{baseUri}scholarshipLocations";
            }
        }
    }
}