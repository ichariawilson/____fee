using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Fee.Services.Applying.Basket.API.Model
{
    public interface IBasketRepository
    {
        Task<StudentBasket> GetBasketAsync(string customerId);
        IEnumerable<string> GetUsers();
        Task<StudentBasket> UpdateBasketAsync(StudentBasket basket);
        Task<bool> DeleteBasketAsync(string id);
    }
}
