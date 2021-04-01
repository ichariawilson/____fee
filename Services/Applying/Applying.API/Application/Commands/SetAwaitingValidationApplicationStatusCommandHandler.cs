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
    public class SetAwaitingValidationApplicationStatusCommandHandler : IRequestHandler<SetAwaitingValidationApplicationStatusCommand, bool>
    {
        private readonly IApplicationRepository _applicationRepository;

        public SetAwaitingValidationApplicationStatusCommandHandler(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        /// <summary>
        /// Handler which processes the command when
        /// graceperiod has finished
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<bool> Handle(SetAwaitingValidationApplicationStatusCommand command, CancellationToken cancellationToken)
        {
            var applicationToUpdate = await _applicationRepository.GetAsync(command.ApplicationNumber);
            if (applicationToUpdate == null)
            {
                return false;
            }

            applicationToUpdate.SetAwaitingValidationStatus();
            return await _applicationRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }


    // Use for Idempotency in Command process
    public class SetAwaitingValidationIdentifiedApplicationStatusCommandHandler : IdentifiedCommandHandler<SetAwaitingValidationApplicationStatusCommand, bool>
    {
        public SetAwaitingValidationIdentifiedApplicationStatusCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<SetAwaitingValidationApplicationStatusCommand, bool>> logger)
            : base(mediator, requestManager, logger)
        {
        }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true;                // Ignore duplicate requests for processing application.
        }
    }
}
