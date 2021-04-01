using MediatR;
using System.Runtime.Serialization;

namespace Applying.API.Application.Commands
{
    public class SetSlotConfirmedApplicationStatusCommand : IRequest<bool>
    {

        [DataMember]
        public int ApplicationNumber { get; private set; }

        public SetSlotConfirmedApplicationStatusCommand(int applicationNumber)
        {
            ApplicationNumber = applicationNumber;
        }
    }
}