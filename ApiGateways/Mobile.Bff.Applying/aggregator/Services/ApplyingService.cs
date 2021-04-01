using GrpcApplying;
using Microsoft.Fee.Mobile.Applying.HttpAggregator.Models;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Fee.Mobile.Applying.HttpAggregator.Services
{
    public class ApplyingService : IApplyingService
    {
        private readonly ApplyingGrpc.ApplyingGrpcClient _applyingGrpcClient;
        private readonly ILogger<ApplyingService> _logger;

        public ApplyingService(ApplyingGrpc.ApplyingGrpcClient applyingGrpcClient, ILogger<ApplyingService> logger)
        {
            _applyingGrpcClient = applyingGrpcClient;
            _logger = logger;
        }

        public async Task<ApplicationData> GetApplicationDraftAsync(BasketData basketData)
        {
            _logger.LogDebug(" grpc client created, basketData={@basketData}", basketData);

            var command = MapToApplicationDraftCommand(basketData);
            var response = await _applyingGrpcClient.CreateApplicationDraftFromBasketDataAsync(command);
            _logger.LogDebug(" grpc response: {@response}", response);

            return MapToResponse(response, basketData);
        }

        private ApplicationData MapToResponse(GrpcApplying.ApplicationDraftDTO applicationDraft, BasketData basketData)
        {
            if (applicationDraft == null)
            {
                return null;
            }

            var data = new ApplicationData
            {
                Student = basketData.StudentId,
                Total = (decimal)applicationDraft.Total,
            };

            applicationDraft.ApplicationItems.ToList().ForEach(o => data.ApplicationItems.Add(new ApplicationItemData
            {
                PictureUrl = o.PictureUrl,
                ScholarshipItemId = o.ScholarshipItemId,
                ScholarshipItemName = o.ScholarshipItemName,
                SlotAmount = (decimal)o.SlotAmount,
                Slots = o.Slots,
            }));

            return data;
        }

        private CreateApplicationDraftCommand MapToApplicationDraftCommand(BasketData basketData)
        {
            var command = new CreateApplicationDraftCommand
            {
                StudentId = basketData.StudentId,
            };

            basketData.Items.ForEach(i => command.Items.Add(new BasketItem
            {
                Id = i.Id,
                OldSlotAmount = (double)i.OldSlotAmount,
                PictureUrl = i.PictureUrl,
                ScholarshipItemId = i.ScholarshipItemId,
                ScholarshipItemName = i.ScholarshipItemName,
                Slots = i.Slots,
                SlotAmount = (double)i.Slots,
            }));

            return command;
        }

    }
}
