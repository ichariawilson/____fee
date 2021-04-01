using Microsoft.Fee.Mobile.Applying.HttpAggregator.Models;
using System.Threading.Tasks;

namespace Microsoft.Fee.Mobile.Applying.HttpAggregator.Services
{
    public interface IApplyingService
    {
        Task<ApplicationData> GetApplicationDraftAsync(BasketData basketData);
    }
}