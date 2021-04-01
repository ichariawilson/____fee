using MediatR;
using System.Runtime.Serialization;

namespace Applying.API.Application.Commands
{
    public class CancelApplicationCommand : IRequest<bool>
    {
        [DataMember]
        public int ApplicationNumber { get; set; }
        public CancelApplicationCommand()
        {

        }
        public CancelApplicationCommand(int applicationNumber)
        {
            ApplicationNumber = applicationNumber;
        }
    }
}
