using Microsoft.Fee.Web.Applying.HttpAggregator.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Fee.Web.Applying.HttpAggregator.Services
{
    public interface IScholarshipService
    {
        Task<ScholarshipItem> GetScholarshipItemAsync(int id);

        Task<IEnumerable<ScholarshipItem>> GetScholarshipItemsAsync(IEnumerable<int> ids);
    }
}
