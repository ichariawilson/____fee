using Microsoft.Fee.Mobile.Applying.HttpAggregator.Models;
using System.Threading.Tasks;

namespace Microsoft.Fee.Mobile.Applying.HttpAggregator.Services
{
    public interface IBasketService
    {
        Task<BasketData> GetById(string id);

        Task UpdateAsync(BasketData currentBasket);

    }
}
