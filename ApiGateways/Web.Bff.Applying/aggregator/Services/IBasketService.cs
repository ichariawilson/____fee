using Microsoft.Fee.Web.Applying.HttpAggregator.Models;
using System.Threading.Tasks;

namespace Microsoft.Fee.Web.Applying.HttpAggregator.Services
{
    public interface IBasketService
    {
        Task<BasketData> GetById(string id);

        Task UpdateAsync(BasketData currentBasket);

    }
}
