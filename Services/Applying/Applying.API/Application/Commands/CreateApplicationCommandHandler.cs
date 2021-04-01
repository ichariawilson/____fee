namespace Microsoft.Fee.Services.Applying.API.Application.Commands
{
    using Domain.AggregatesModel.ApplicationAggregate;
    using global::Applying.API.Application.IntegrationEvents;
    using global::Applying.API.Application.IntegrationEvents.Events;
    using MediatR;
    using Microsoft.Fee.Services.Applying.API.Infrastructure.Services;
    using Microsoft.Fee.Services.Applying.Infrastructure.Idempotency;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    // Regular CommandHandler
    public class CreateApplicationCommandHandler
        : IRequestHandler<CreateApplicationCommand, bool>
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IIdentityService _identityService;
        private readonly IMediator _mediator;
        private readonly IApplyingIntegrationEventService _applyingIntegrationEventService;
        private readonly ILogger<CreateApplicationCommandHandler> _logger;

        // Using DI to inject infrastructure persistence Repositories
        public CreateApplicationCommandHandler(IMediator mediator,
            IApplyingIntegrationEventService applyingIntegrationEventService,
            IApplicationRepository applicationRepository,
            IIdentityService identityService,
            ILogger<CreateApplicationCommandHandler> logger)
        {
            _applicationRepository = applicationRepository ?? throw new ArgumentNullException(nameof(applicationRepository));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _applyingIntegrationEventService = applyingIntegrationEventService ?? throw new ArgumentNullException(nameof(applyingIntegrationEventService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(CreateApplicationCommand message, CancellationToken cancellationToken)
        {
            // Add Integration event to clean the basket
            var applicationStartedIntegrationEvent = new ApplicationStartedIntegrationEvent(message.UserId);
            await _applyingIntegrationEventService.AddAndSaveEventAsync(applicationStartedIntegrationEvent);

            // Add/Update the Student AggregateRoot
            // DDD patterns comment: Add child entities and value-objects through the Application Aggregate-Root
            // methods and constructor so validations, invariants and business logic 
            // make sure that consistency is preserved across the whole aggregate
            var profile = new Profile(message.IDNumber, message.Request);
            var application = new Application(message.UserId, message.UserName, profile, message.PaymentTypeId);

            foreach (var item in message.ApplicationItems)
            {
                application.AddApplicationItem(item.ScholarshipItemId, item.ScholarshipItemName, item.SlotAmount, item.PictureUrl, item.Slots);
            }

            _logger.LogInformation("----- Creating Application - Application: {@Application}", application);

            _applicationRepository.Add(application);

            return await _applicationRepository.UnitOfWork
                .SaveEntitiesAsync(cancellationToken);
        }
    }

    // Use for Idempotency in Command process
    public class CreateApplicationIdentifiedCommandHandler : IdentifiedCommandHandler<CreateApplicationCommand, bool>
    {
        public CreateApplicationIdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<CreateApplicationCommand, bool>> logger)
            : base(mediator, requestManager, logger)
        {
        }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true;                // Ignore duplicate requests for creating application.
        }
    }
}
