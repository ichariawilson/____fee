using System.ComponentModel.DataAnnotations.Schema;

namespace Microsoft.Fee.Services.Scholarship.API.Model
{
    public class ScholarshipCourse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Fee { get; set; }

        public int ScholarshipDurationId { get; set; }

        public ScholarshipDuration ScholarshipDuration { get; set; }
    }
}
