using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Fee.Web.Applying.HttpAggregator.Models;
using Microsoft.Fee.Web.Applying.HttpAggregator.Services;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Microsoft.Fee.Web.Applying.HttpAggregator.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IScholarshipService _scholarship;
        private readonly IBasketService _basket;

        public BasketController(IScholarshipService scholarshipService, IBasketService basketService)
        {
            _scholarship = scholarshipService;
            _basket = basketService;
        }

        [HttpPost]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BasketData), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketData>> UpdateAllBasketAsync([FromBody] UpdateBasketRequest data)
        {
            if (data.Items == null || !data.Items.Any())
            {
                return BadRequest("Need to pass at least one basket line");
            }

            // Retrieve the current basket
            var basket = await _basket.GetById(data.StudentId) ?? new BasketData(data.StudentId);
            var scholarshipItems = await _scholarship.GetScholarshipItemsAsync(data.Items.Select(x => x.ScholarshipItemId));

            // group by Scholarship Item id to avoid duplicates
            var itemsCalculated = data
                    .Items
                    .GroupBy(x => x.ScholarshipItemId, x => x, (k, i) => new { scholarshipItemId = k, items = i })
                    .Select(groupedItem =>
                    {
                        var item = groupedItem.items.First();
                        item.Slots = groupedItem.items.Sum(i => i.Slots);
                        return item;
                    });

            foreach (var bitem in itemsCalculated)
            {
                var scholarshipItem = scholarshipItems.SingleOrDefault(ci => ci.Id == bitem.ScholarshipItemId);
                if (scholarshipItem == null)
                {
                    return BadRequest($"Basket refers to a non-existing scholarship item ({bitem.ScholarshipItemId})");
                }

                var itemInBasket = basket.Items.FirstOrDefault(x => x.ScholarshipItemId == bitem.ScholarshipItemId);
                if (itemInBasket == null)
                {
                    basket.Items.Add(new BasketDataItem()
                    {
                        Id = bitem.Id,
                        ScholarshipItemId = scholarshipItem.Id,
                        ScholarshipItemName = scholarshipItem.Name,
                        PictureUrl = scholarshipItem.PictureUri,
                        SlotAmount = scholarshipItem.Amount,
                        Slots = bitem.Slots
                    });
                }
                else
                {
                    itemInBasket.Slots = bitem.Slots;
                }
            }

            await _basket.UpdateAsync(basket);

            return basket;
        }

        [HttpPut]
        [Route("items")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BasketData), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketData>> UpdateSlotsAsync([FromBody] UpdateBasketItemsRequest data)
        {
            if (!data.Updates.Any())
            {
                return BadRequest("No updates sent");
            }

            // Retrieve the current basket
            var currentBasket = await _basket.GetById(data.BasketId);
            if (currentBasket == null)
            {
                return BadRequest($"Basket with id {data.BasketId} not found.");
            }

            // Update with new slots
            foreach (var update in data.Updates)
            {
                var basketItem = currentBasket.Items.SingleOrDefault(bitem => bitem.Id == update.BasketItemId);
                if (basketItem == null)
                {
                    return BadRequest($"Basket item with id {update.BasketItemId} not found");
                }
                basketItem.Slots = update.NewSlts;
            }

            // Save the updated basket
            await _basket.UpdateAsync(currentBasket);

            return currentBasket;
        }

        [HttpPost]
        [Route("items")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> AddBasketItemAsync([FromBody] AddBasketItemRequest data)
        {
            if (data == null || data.Slots == 0)
            {
                return BadRequest("Invalid payload");
            }

            // Step 1: Get the item from scholarship
            var item = await _scholarship.GetScholarshipItemAsync(data.ScholarshipItemId);

            //item.PictureUri = 

            // Step 2: Get current basket status
            var currentBasket = (await _basket.GetById(data.BasketId)) ?? new BasketData(data.BasketId);
            // Step 3: Search if exist scholarshipItem into basket
            var scholarshipItem = currentBasket.Items.SingleOrDefault(i => i.ScholarshipItemId == item.Id);
            if (scholarshipItem != null)
            {
                // Step 4: Update quantity for scholarshipItem
                scholarshipItem.Slots += data.Slots;
            }
            else
            {
                // Step 4: Merge current status with new scholarshipItem
                currentBasket.Items.Add(new BasketDataItem()
                {
                    SlotAmount = item.Amount,
                    PictureUrl = item.PictureUri,
                    ScholarshipItemId = item.Id,
                    ScholarshipItemName = item.Name,
                    Slots = data.Slots,
                    Id = Guid.NewGuid().ToString()
                });
            }

            // Step 5: Update basket
            await _basket.UpdateAsync(currentBasket);

            return Ok();
        }
    }
}
