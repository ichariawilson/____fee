using System.Collections.Generic;

namespace Microsoft.Fee.Web.Scholarship.HttpAggregator.Config
{
    public class UrlsConfig
    {
        public class ScholarshipOperations
        {
            public static string GetItemById(int id) => $"/api/v1/scholarship/items/{id}";

            public static string GetItemsById(IEnumerable<int> ids) => $"/api/v1/scholarship/items?ids={string.Join(',', ids)}";
        }

        public class ApplicationsOperations
        {
            public static string GetApplicationCancel() => "/api/v1/applications/cancel";
            public static string GetApplicationGrant() => "/api/v1/applications/grant";
        }

        public string Scholarship { get; set; }

        public string Applications { get; set; }

        public string GrpcScholarship { get; set; }

        public string GrpcApplying { get; set; }
    }
}
