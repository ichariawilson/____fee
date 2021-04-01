namespace Microsoft.Fee.Services.Applying.API.Application.Commands
{
    using Domain.AggregatesModel.ApplicationAggregate;
    using global::Applying.API.Application.Models;
    using MediatR;
    using Microsoft.Fee.Services.Applying.API.Infrastructure.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using static Microsoft.Fee.Services.Applying.API.Application.Commands.CreateApplicationCommand;

    // Regular CommandHandler
    public class CreateApplicationDraftCommandHandler
        : IRequestHandler<CreateApplicationDraftCommand, ApplicationDraftDTO>
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IIdentityService _identityService;
        private readonly IMediator _mediator;

        // Using DI to inject infrastructure persistence Repositories
        public CreateApplicationDraftCommandHandler(IMediator mediator, IIdentityService identityService)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public Task<ApplicationDraftDTO> Handle(CreateApplicationDraftCommand message, CancellationToken cancellationToken)
        {

            var application = Application.NewDraft();
            var applicationItems = message.Items.Select(i => i.ToApplicationItemDTO());
            foreach (var item in applicationItems)
            {
                application.AddApplicationItem(item.ScholarshipItemId, item.ScholarshipItemName, item.SlotAmount, item.PictureUrl, item.Slots);
            }

            return Task.FromResult(ApplicationDraftDTO.FromApplication(application));
        }
    }


    public record ApplicationDraftDTO
    {
        public IEnumerable<ApplicationItemDTO> ApplicationItems { get; init; }
        public decimal Total { get; init; }

        public static ApplicationDraftDTO FromApplication(Application application)
        {
            return new ApplicationDraftDTO()
            {
                ApplicationItems = application.ApplicationItems.Select(ai => new ApplicationItemDTO
                {
                    ScholarshipItemId = ai.ScholarshipItemId,
                    SlotAmount = ai.GetSlotAmount(),
                    PictureUrl = ai.GetPictureUri(),
                    Slots = ai.GetSlots(),
                    ScholarshipItemName = ai.GetApplicationItemScholarshipItemName()
                }),
                Total = application.GetTotal()
            };
        }
    }
}
