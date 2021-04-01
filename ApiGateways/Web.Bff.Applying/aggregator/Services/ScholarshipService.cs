using ScholarshipApi;
using Microsoft.Fee.Web.Applying.HttpAggregator.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Fee.Web.Applying.HttpAggregator.Services
{
    public class ScholarshipService : IScholarshipService
    {
        private readonly Scholarship.ScholarshipClient _client;

        public ScholarshipService(Scholarship.ScholarshipClient client)
        {
            _client = client;
        }

        public async Task<ScholarshipItem> GetScholarshipItemAsync(int id)
        {
            var request = new ScholarshipItemRequest { Id = id };
            var response = await _client.GetItemByIdAsync(request);
            return MapToScholarshipItemResponse(response);
        }

        public async Task<IEnumerable<ScholarshipItem>> GetScholarshipItemsAsync(IEnumerable<int> ids)
        {
            var request = new ScholarshipItemsRequest { Ids = string.Join(",", ids), PageIndex = 1, PageSize = 10 };
            var response = await _client.GetItemsByIdsAsync(request);
            return response.Data.Select(MapToScholarshipItemResponse);
        }

        private ScholarshipItem MapToScholarshipItemResponse(ScholarshipItemResponse scholarshipItemResponse)
        {
            return new ScholarshipItem
            {
                Id = scholarshipItemResponse.Id,
                Name = scholarshipItemResponse.Name,
                PictureUri = scholarshipItemResponse.PictureUri,
                Amount = (decimal)scholarshipItemResponse.Amount
            };
        }
    }
}
