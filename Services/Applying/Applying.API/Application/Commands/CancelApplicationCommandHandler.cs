using MediatR;
using Microsoft.Fee.Services.Applying.API.Application.Commands;
using Microsoft.Fee.Services.Applying.Domain.AggregatesModel.ApplicationAggregate;
using Microsoft.Fee.Services.Applying.Infrastructure.Idempotency;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Applying.API.Application.Commands
{
    // Regular CommandHandler
    public class CancelApplicationCommandHandler : IRequestHandler<CancelApplicationCommand, bool>
    {
        private readonly IApplicationRepository _applicationRepository;

        public CancelApplicationCommandHandler(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        /// <summary>
        /// Handler which processes the command when
        /// student executes cancel application from app
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<bool> Handle(CancelApplicationCommand command, CancellationToken cancellationToken)
        {
            var applicationToUpdate = await _applicationRepository.GetAsync(command.ApplicationNumber);
            if (applicationToUpdate == null)
            {
                return false;
            }

            applicationToUpdate.SetCancelledStatus();
            return await _applicationRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }


    // Use for Idempotency in Command process
    public class CancelApplicationIdentifiedCommandHandler : IdentifiedCommandHandler<CancelApplicationCommand, bool>
    {
        public CancelApplicationIdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<CancelApplicationCommand, bool>> logger)
            : base(mediator, requestManager, logger)
        {
        }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true;                // Ignore duplicate requests for processing application.
        }
    }
}
