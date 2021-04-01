using MediatR;
using System.Runtime.Serialization;

namespace Applying.API.Application.Commands
{
    public class SetPaidApplicationStatusCommand : IRequest<bool>
    {

        [DataMember]
        public int ApplicationNumber { get; private set; }

        public SetPaidApplicationStatusCommand(int applicationNumber)
        {
            ApplicationNumber = applicationNumber;
        }
    }
}