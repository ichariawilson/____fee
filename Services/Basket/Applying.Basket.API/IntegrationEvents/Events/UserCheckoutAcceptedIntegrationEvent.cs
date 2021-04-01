using Microsoft.Fee.BuildingBlocks.EventBus.Events;
using Microsoft.Fee.Services.Applying.Basket.API.Model;
using System;

namespace Applying.Basket.API.IntegrationEvents.Events
{
    public record UserCheckoutAcceptedIntegrationEvent : IntegrationEvent
    {
        public string UserId { get; }

        public string UserName { get; }

        public int ApplicationNumber { get; init; }

        public string IDNumber { get; init; }

        public decimal Request { get; init; }

        public int PaymentTypeId { get; init; }

        public int GenderId { get; init; }

        public string Student { get; init; }

        public Guid RequestId { get; init; }

        public StudentBasket Basket { get; }

        public UserCheckoutAcceptedIntegrationEvent(string userId, string userName, string idNumber, decimal request, 
            int paymentTypeId, int genderid, string student, Guid requestId, StudentBasket basket)
        {
            UserId = userId;
            UserName = userName;
            IDNumber = idNumber;
            Request = request;
            PaymentTypeId = paymentTypeId;
            GenderId = genderid;
            Student = student;
            Basket = basket;
            RequestId = requestId;
        }
    }
}
