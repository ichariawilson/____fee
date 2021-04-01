using Microsoft.Fee.Web.Scholarship.HttpAggregator.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Fee.Web.Scholarship.HttpAggregator.Services
{
    public interface IScholarshipService
    {
        Task<ScholarshipItem> GetScholarshipItemAsync(int id);

        Task<IEnumerable<ScholarshipItem>> GetScholarshipItemsAsync(IEnumerable<int> ids);
    }
}
