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
    public class GrantApplicationCommandHandler : IRequestHandler<GrantApplicationCommand, bool>
    {
        private readonly IApplicationRepository _applicationRepository;

        public GrantApplicationCommandHandler(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        /// <summary>
        /// Handler which processes the command when
        /// administrator executes grant application from app
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<bool> Handle(GrantApplicationCommand command, CancellationToken cancellationToken)
        {
            var applicationToUpdate = await _applicationRepository.GetAsync(command.ApplicationNumber);
            if (applicationToUpdate == null)
            {
                return false;
            }

            applicationToUpdate.SetGrantedStatus();
            return await _applicationRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }


    // Use for Idempotency in Command process
    public class GrantApplicationIdentifiedCommandHandler : IdentifiedCommandHandler<GrantApplicationCommand, bool>
    {
        public GrantApplicationIdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<GrantApplicationCommand, bool>> logger)
            : base(mediator, requestManager, logger)
        {
        }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true;                // Ignore duplicate requests for processing application.
        }
    }
}
