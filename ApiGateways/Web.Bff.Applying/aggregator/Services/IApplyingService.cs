using Microsoft.Fee.Web.Applying.HttpAggregator.Models;
using System.Threading.Tasks;

namespace Microsoft.Fee.Web.Applying.HttpAggregator.Services
{
    public interface IApplyingService
    {
        Task<ApplicationData> GetApplicationDraftAsync(BasketData basketData);
    }
}