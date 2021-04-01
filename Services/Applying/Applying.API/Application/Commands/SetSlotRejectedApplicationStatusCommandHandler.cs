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
    public class SetSlotRejectedApplicationStatusCommandHandler : IRequestHandler<SetSlotRejectedApplicationStatusCommand, bool>
    {
        private readonly IApplicationRepository _applicationRepository;

        public SetSlotRejectedApplicationStatusCommandHandler(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        /// <summary>
        /// Handler which processes the command when
        /// Slot service rejects the request
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<bool> Handle(SetSlotRejectedApplicationStatusCommand command, CancellationToken cancellationToken)
        {
            // Simulate a work time for rejecting the stock
            await Task.Delay(10000, cancellationToken);

            var applicationToUpdate = await _applicationRepository.GetAsync(command.ApplicationNumber);
            if (applicationToUpdate == null)
            {
                return false;
            }

            applicationToUpdate.SetCancelledStatusWhenSlotIsRejected(command.ApplicationSlotItems);

            return await _applicationRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }


    // Use for Idempotency in Command process
    public class SetSlotRejectedApplicationStatusIdenfifiedCommandHandler : IdentifiedCommandHandler<SetSlotRejectedApplicationStatusCommand, bool>
    {
        public SetSlotRejectedApplicationStatusIdenfifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<SetSlotRejectedApplicationStatusCommand, bool>> logger)
            : base(mediator, requestManager, logger)
        {
        }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true;                // Ignore duplicate requests for processing application.
        }
    }
}
