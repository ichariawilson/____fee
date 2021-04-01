using Google.Protobuf.Collections;
using Grpc.Core;
using MediatR;
using Microsoft.Fee.BuildingBlocks.EventBus.Extensions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiModels = Applying.API.Application.Models;
using AppCommand = Microsoft.Fee.Services.Applying.API.Application.Commands;

namespace GrpcApplying
{
    public class ApplyingService : ApplyingGrpc.ApplyingGrpcBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ApplyingService> _logger;

        public ApplyingService(IMediator mediator, ILogger<ApplyingService> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public override async Task<ApplicationDraftDTO> CreateApplicationDraftFromBasketData(CreateApplicationDraftCommand createApplicationDraftCommand, ServerCallContext context)
        {
            _logger.LogInformation("Begin grpc call from method {Method} for applying get application draft {CreateApplicationDraftCommand}", context.Method, createApplicationDraftCommand);
            _logger.LogTrace(
                "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                createApplicationDraftCommand.GetGenericTypeName(),
                nameof(createApplicationDraftCommand.StudentId),
                createApplicationDraftCommand.StudentId,
                createApplicationDraftCommand);

            var command = new AppCommand.CreateApplicationDraftCommand(
                            createApplicationDraftCommand.StudentId,
                            this.MapBasketItems(createApplicationDraftCommand.Items));


            var data = await _mediator.Send(command);

            if (data != null)
            {
                context.Status = new Status(StatusCode.OK, $" applying get application draft {createApplicationDraftCommand} do exist");

                return this.MapResponse(data);
            }
            else
            {
                context.Status = new Status(StatusCode.NotFound, $" applying get application draft {createApplicationDraftCommand} do not exist");
            }

            return new ApplicationDraftDTO();
        }

        public ApplicationDraftDTO MapResponse(AppCommand.ApplicationDraftDTO application)
        {
            var result = new ApplicationDraftDTO()
            {
                Total = (double)application.Total,
            };

            application.ApplicationItems.ToList().ForEach(i => result.ApplicationItems.Add(new ApplicationItemDTO()
            {
                PictureUrl = i.PictureUrl,
                ScholarshipItemId = i.ScholarshipItemId,
                ScholarshipItemName = i.ScholarshipItemName,
                SlotAmount = (double)i.SlotAmount,
                Slots = i.Slots,
            }));

            return result;
        }

        public IEnumerable<ApiModels.BasketItem> MapBasketItems(RepeatedField<BasketItem> items)
        {
            return items.Select(x => new ApiModels.BasketItem()
            {
                Id = x.Id,
                ScholarshipItemId = x.ScholarshipItemId,
                ScholarshipItemName = x.ScholarshipItemName,
                SlotAmount = (decimal)x.SlotAmount,
                OldSlotAmount = (decimal)x.OldSlotAmount,
                Slots = x.Slots,
                PictureUrl = x.PictureUrl,
            });
        }
    }
}
