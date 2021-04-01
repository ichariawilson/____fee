using MediatR;
using System.Runtime.Serialization;

namespace Applying.API.Application.Commands
{
    public class GrantApplicationCommand : IRequest<bool>
    {

        [DataMember]
        public int ApplicationNumber { get; private set; }

        public GrantApplicationCommand(int applicationNumber)
        {
            ApplicationNumber = applicationNumber;
        }
    }
}