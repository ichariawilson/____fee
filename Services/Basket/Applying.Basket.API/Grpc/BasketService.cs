using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Fee.Services.Applying.Basket.API.Model;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcBasket
{
    public class BasketService : Basket.BasketBase
    {
        private readonly IBasketRepository _repository;
        private readonly ILogger<BasketService> _logger;

        public BasketService(IBasketRepository repository, ILogger<BasketService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [AllowAnonymous]
        public override async Task<StudentBasketResponse> GetBasketById(BasketRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Begin grpc call from method {Method} for basket id {Id}", context.Method, request.Id);

            var data = await _repository.GetBasketAsync(request.Id);

            if (data != null)
            {
                context.Status = new Status(StatusCode.OK, $"Basket with id {request.Id} do exist");

                return MapToStudentBasketResponse(data);
            }
            else
            {
                context.Status = new Status(StatusCode.NotFound, $"Basket with id {request.Id} do not exist");
            }

            return new StudentBasketResponse();
        }

        public override async Task<StudentBasketResponse> UpdateBasket(StudentBasketRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Begin grpc call BasketService.UpdateBasketAsync for student id {Studentid}", request.Studentid);

            var studentBasket = MapToStudentBasket(request);

            var response = await _repository.UpdateBasketAsync(studentBasket);

            if (response != null)
            {
                return MapToStudentBasketResponse(response);
            }

            context.Status = new Status(StatusCode.NotFound, $"Basket with student id {request.Studentid} do not exist");

            return null;
        }

        private StudentBasketResponse MapToStudentBasketResponse(StudentBasket studentBasket)
        {
            var response = new StudentBasketResponse
            {
                Studentid = studentBasket.StudentId
            };

            studentBasket.Items.ForEach(item => response.Items.Add(new BasketItemResponse
            {
                Id = item.Id,
                Oldslotamount = (double)item.OldSlotAmount,
                Pictureurl = item.PictureUrl,
                Scholarshipitemid = item.ScholarshipItemId,
                ScholarshipItemname = item.ScholarshipItemName,
                Slots = item.Slots,
                Slotamount = (double)item.SlotAmount
            }));

            return response;
        }

        private StudentBasket MapToStudentBasket(StudentBasketRequest studentBasketRequest)
        {
            var response = new StudentBasket
            {
                StudentId = studentBasketRequest.Studentid
            };

            studentBasketRequest.Items.ToList().ForEach(item => response.Items.Add(new BasketItem
            {
                Id = item.Id,
                OldSlotAmount = (decimal)item.Oldslotamount,
                PictureUrl = item.Pictureurl,
                ScholarshipItemId = item.Scholarshipitemid,
                ScholarshipItemName = item.ScholarshipItemname,
                Slots = item.Slots,
                SlotAmount = (decimal)item.Slotamount
            }));

            return response;
        }
    }
}