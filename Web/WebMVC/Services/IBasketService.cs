using Microsoft.Fee.WebMVC.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebMVC.Services.ModelDTOs;

namespace Microsoft.Fee.WebMVC.Services
{
    public interface IBasketService
    {
        Task<Basket> GetBasket(ApplicationUser user);
        Task AddItemToBasket(ApplicationUser user, int scholarshipItemId);
        Task<Basket> UpdateBasket(Basket basket);
        Task Checkout(BasketDTO basket);
        Task<Basket> SetSlots(ApplicationUser user, Dictionary<string, int> slots);
        Task<Application> GetApplicationDraft(string basketId);
    }
}
