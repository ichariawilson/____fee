using Scholarship.API.Infrastructure.Exceptions;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microsoft.Fee.Services.Scholarship.API.Model
{
    public class ScholarshipItem
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Scholarship Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        //public DateTime CreateDate { get; set; }

        //public DateTime Deadline { get; set; }

        [Required]
        [Display(Name = "Amount")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Amount { get; set; }

        public string PictureFileName { get; set; }

        [Required]
        [Display(Name = "Scholarship Flag")]
        public string PictureUri { get; set; }

        public int ScholarshipCurrencyId { get; set; }

        public ScholarshipCurrency ScholarshipCurrency { get; set; }

        public int ScholarshipDurationId { get; set; }

        public ScholarshipDuration ScholarshipDuration { get; set; }

        public int ScholarshipEducationLevelId { get; set; }

        public ScholarshipEducationLevel ScholarshipEducationLevel { get; set; }

        public int ScholarshipInterestId { get; set; }

        public ScholarshipInterest ScholarshipInterest { get; set; }

        public int ScholarshipLocationId { get; set; }

        public ScholarshipLocation ScholarshipLocation { get; set; }

        // Slots
        public int AvailableSlots { get; set; }

        // Available slot at which we should reorder
        public int ReslotThreshold { get; set; }


        // Maximum number of slots at any time
        public int MaxSlotThreshold { get; set; }

        /// <summary>
        /// True if item is on reorder
        /// </summary>
        public bool OnReapply { get; set; }

        public ScholarshipItem() { }


        /// <summary>
        /// Decrements the slots and ensures the reslotThreshold hasn't
        /// been breached. If so, a ReslotRequest is generated in CheckThreshold. 
        /// 
        /// If there is sufficient slots of an item, then the integer returned at the end of this call should be the same as slotsDesired. 
        /// In the event that there is not sufficient slots available, the method will remove whatever slots are available and return those slots to the client.
        /// In this case, it is the responsibility of the client to determine if the amount that is returned is the same as slotsDesired.
        /// It is invalid to pass in a negative number. 
        /// </summary>
        /// <param name="slotsDesired"></param>
        /// <returns>int: Returns the number actually removed from slots. </returns>
        /// 
        public int RemoveSlots(int slotsDesired)
        {
            if (AvailableSlots == 0)
            {
                throw new ScholarshipDomainException($"Empty slots, scholarship item {Name} is unavailable");
            }

            if (slotsDesired <= 0)
            {
                throw new ScholarshipDomainException($"Slots desired should be greater than zero");
            }

            int removed = Math.Min(slotsDesired, this.AvailableSlots);

            this.AvailableSlots -= removed;

            return removed;
        }

        /// <summary>
        /// Increments the quantity of a particular item in inventory.
        /// <param name="quantity"></param>
        /// <returns>int: Returns the quantity that has been added to slots</returns>
        /// </summary>
        public int AddSlots(int quantity)
        {
            int original = this.AvailableSlots;

            // The quantity that the client is trying to add to slots is greater than what can be physically accommodated in the Warehouse
            if ((this.AvailableSlots + quantity) > this.MaxSlotThreshold)
            {
                // For now, this method only adds new slots up to maximum slots threshold. In an expanded version of this application. 
                this.AvailableSlots += (this.MaxSlotThreshold - this.AvailableSlots);
            }
            else
            {
                this.AvailableSlots += quantity;
            }

            this.OnReapply = false;

            return this.AvailableSlots - original;
        }
    }
}