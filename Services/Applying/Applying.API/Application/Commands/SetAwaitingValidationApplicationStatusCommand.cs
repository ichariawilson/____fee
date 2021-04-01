using MediatR;
using System.Runtime.Serialization;

namespace Applying.API.Application.Commands
{
    public class SetAwaitingValidationApplicationStatusCommand : IRequest<bool>
    {

        [DataMember]
        public int ApplicationNumber { get; private set; }

        public SetAwaitingValidationApplicationStatusCommand(int applicationNumber)
        {
            ApplicationNumber = applicationNumber;
        }
    }
}