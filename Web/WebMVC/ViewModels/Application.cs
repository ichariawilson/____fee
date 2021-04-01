using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebMVC.Services.ModelDTOs;

namespace Microsoft.Fee.WebMVC.ViewModels
{
    public class Application
    {
        public string ApplicationNumber { get; set; }

        public DateTime Date { get; set; }

        public string Status { get; set; }

        public decimal Total { get; set; }

        public string Description { get; set; }
        [Required]
        public string DateofBirth { get; set; }
        [Required]
        public string IDNumber { get; set; }
        [Required]
        public decimal Request { get; set; }
        [Required]
        public int GenderId { get; set; }
        [Required]
        public int HobbyId { get; set; }
        [Required]
        public int LocationIdId { get; set; }
        [Required]
        public int SchoolId { get; set; }

        public int PaymentTypeId { get; set; }

        public string Student { get; set; }

        public List<SelectListItem> ActionCodeSelectList =>
           GetActionCodesByCurrentState();

        // See the property initializer syntax below. This
        // initializes the compiler generated field for this
        // auto-implemented property.
        public List<ApplicationItem> ApplicationItems { get; } = new List<ApplicationItem>();

        [Required]
        public Guid RequestId { get; set; }

        private List<SelectListItem> GetActionCodesByCurrentState()
        {
            var actions = new List<ApplicationProcessAction>();
            switch (Status?.ToLower())
            {
                case "paid":
                    actions.Add(ApplicationProcessAction.Grant);
                    break;
            }

            var result = new List<SelectListItem>();
            actions.ForEach(action =>
            {
                result.Add(new SelectListItem { Text = action.Name, Value = action.Code });
            });

            return result;
        }
    }

    public enum paymentType
    {
        MPesa = 1
    }
}
