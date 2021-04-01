using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Applying.API.Application.Commands
{
    public class SetSlotRejectedApplicationStatusCommand : IRequest<bool>
    {

        [DataMember]
        public int ApplicationNumber { get; private set; }

        [DataMember]
        public List<int> ApplicationSlotItems { get; private set; }

        public SetSlotRejectedApplicationStatusCommand(int applicationNumber, List<int> applicationSlotItems)
        {
            ApplicationNumber = applicationNumber;
            ApplicationSlotItems = applicationSlotItems;
        }
    }
}