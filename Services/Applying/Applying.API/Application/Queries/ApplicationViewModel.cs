using System;
using System.Collections.Generic;

namespace Microsoft.Fee.Services.Applying.API.Application.Queries
{
    public record Applicationitem
    {
        public string scholarshipitemname { get; init; }
        public int slots { get; init; }
        public double slotamount { get; init; }
        public string pictureurl { get; init; }
    }

    public record Application
    {
        public int applicationnumber { get; init; }
        public DateTime date { get; init; }
        public string status { get; init; }
        public string description { get; init; }
        public string idnumber { get; init; }
        public decimal request { get; init; }
        public List<Applicationitem> applicationitems { get; set; }
        public decimal total { get; set; }
    }

    public record ApplicationSummary
    {
        public int applicationnumber { get; init; }
        public DateTime date { get; init; }
        public string status { get; init; }
        public double total { get; init; }
    }

    public record PaymentType
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }
}
