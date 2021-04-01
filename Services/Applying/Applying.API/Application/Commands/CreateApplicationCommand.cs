using MediatR;
using Applying.API.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Microsoft.Fee.Services.Applying.API.Application.Commands
{
    [DataContract]
    public class CreateApplicationCommand : IRequest<bool>
    {
        [DataMember]
        private readonly List<ApplicationItemDTO> _applicationItems;

        [DataMember]
        public string UserId { get; private set; }

        [DataMember]
        public string UserName { get; private set; }

        [DataMember]
        public string IDNumber { get; private set; }

        [DataMember]
        public decimal Request { get; private set; }

        [DataMember]
        public int PaymentTypeId { get; private set; }

        [DataMember]
        public IEnumerable<ApplicationItemDTO> ApplicationItems => _applicationItems;

        public CreateApplicationCommand()
        {
            _applicationItems = new List<ApplicationItemDTO>();
        }

        public CreateApplicationCommand(List<BasketItem> basketItems, string userId, string userName, string idNumber, decimal request, 
            int paymentTypeId) : this()
        {
            _applicationItems = basketItems.ToApplicationItemsDTO().ToList();
            UserId = userId;
            UserName = userName;
            IDNumber = idNumber;
            Request = request;
            PaymentTypeId = paymentTypeId;
        }


        public record ApplicationItemDTO
        {
            public int ScholarshipItemId { get; init; }

            public string ScholarshipItemName { get; init; }

            public decimal SlotAmount { get; init; }

            public int Slots { get; init; }

            public string PictureUrl { get; init; }
        }
    }
}
