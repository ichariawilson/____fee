using Microsoft.Fee.BuildingBlocks.EventBus.Events;
using Applying.API.Application.Models;
using System;

namespace Applying.API.Application.IntegrationEvents.Events
{
    public record UserCheckoutAcceptedIntegrationEvent : IntegrationEvent
    {
        public string UserId { get; }

        public string UserName { get; }

        public string IDNumber { get; }

        public decimal Request { get; }

        public int PaymentTypeId { get; set; }

        public int GenderId { get; set; }

        public string Student { get; set; }

        public Guid RequestId { get; set; }

        public StudentBasket Basket { get; }

        public UserCheckoutAcceptedIntegrationEvent(string userId, string userName, string idNumber, decimal request, int paymentTypeId, string student, Guid requestId,
            StudentBasket basket)
        {
            UserId = userId;
            IDNumber = idNumber;
            Request = request;
            PaymentTypeId = paymentTypeId;
            Student = student;
            Basket = basket;
            RequestId = requestId;
            UserName = userName;
        }

    }
}
