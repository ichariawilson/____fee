namespace Microsoft.Fee.Services.Scholarship.API.Model
{
    public class ScholarshipSchool
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }

        public int ScholarshipLocationId { get; set; }

        public ScholarshipLocation ScholarshipLocation { get; set; }
    }
}
