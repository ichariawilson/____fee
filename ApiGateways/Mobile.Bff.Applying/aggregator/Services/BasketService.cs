using GrpcBasket;
using Microsoft.Fee.Mobile.Applying.HttpAggregator.Models;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Fee.Mobile.Applying.HttpAggregator.Services
{
    public class BasketService : IBasketService
    {
        private readonly Basket.BasketClient _basketClient;
        private readonly ILogger<BasketService> _logger;

        public BasketService(Basket.BasketClient basketClient, ILogger<BasketService> logger)
        {
            _basketClient = basketClient;
            _logger = logger;
        }

        public async Task<BasketData> GetById(string id)
        {
            _logger.LogDebug("grpc client created, request = {@id}", id);
            var response = await _basketClient.GetBasketByIdAsync(new BasketRequest { Id = id });
            _logger.LogDebug("grpc response {@response}", response);

            return MapToBasketData(response);
        }

        public async Task UpdateAsync(BasketData currentBasket)
        {
            _logger.LogDebug("Grpc update basket currentBasket {@currentBasket}", currentBasket);
            var request = MapToStudentBasketRequest(currentBasket);
            _logger.LogDebug("Grpc update basket request {@request}", request);

            await _basketClient.UpdateBasketAsync(request);
        }

        private BasketData MapToBasketData(StudentBasketResponse studentBasketRequest)
        {
            if (studentBasketRequest == null)
            {
                return null;
            }

            var map = new BasketData
            {
                StudentId = studentBasketRequest.Studentid
            };

            studentBasketRequest.Items.ToList().ForEach(item => map.Items.Add(new BasketDataItem
            {
                Id = item.Id,
                OldSlotAmount = (decimal)item.Oldslotamount,
                PictureUrl = item.Pictureurl,
                ScholarshipItemId = item.Scholarshipitemid,
                ScholarshipItemName = item.ScholarshipItemname,
                Slots = item.Slots,
                SlotAmount = (decimal)item.Slotamount
            }));

            return map;
        }

        private StudentBasketRequest MapToStudentBasketRequest(BasketData basketData)
        {
            if (basketData == null)
            {
                return null;
            }

            var map = new StudentBasketRequest
            {
                Studentid = basketData.StudentId
            };

            basketData.Items.ToList().ForEach(item => map.Items.Add(new BasketItemResponse
            {
                Id = item.Id,
                Oldslotamount = (double)item.OldSlotAmount,
                Pictureurl = item.PictureUrl,
                Scholarshipitemid = item.ScholarshipItemId,
                ScholarshipItemname = item.ScholarshipItemName,
                Slots = item.Slots,
                Slotamount = (double)item.SlotAmount
            }));

            return map;
        }
    }
}
