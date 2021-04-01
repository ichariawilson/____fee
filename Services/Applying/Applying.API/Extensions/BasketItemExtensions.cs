using System.Collections.Generic;
using static Microsoft.Fee.Services.Applying.API.Application.Commands.CreateApplicationCommand;

namespace Applying.API.Application.Models
{
    public static class BasketItemExtensions
    {
        public static IEnumerable<ApplicationItemDTO> ToApplicationItemsDTO(this IEnumerable<BasketItem> basketItems)
        {
            foreach (var item in basketItems)
            {
                yield return item.ToApplicationItemDTO();
            }
        }

        public static ApplicationItemDTO ToApplicationItemDTO(this BasketItem item)
        {
            return new ApplicationItemDTO()
            {
                ScholarshipItemId = item.ScholarshipItemId,
                ScholarshipItemName = item.ScholarshipItemName,
                PictureUrl = item.PictureUrl,
                SlotAmount = item.SlotAmount,
                Slots = item.Slots
            };
        }
    }
}
