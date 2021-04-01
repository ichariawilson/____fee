using System.ComponentModel.DataAnnotations.Schema;

namespace Microsoft.Fee.Services.Scholarship.API.Model
{
    public class ScholarshipFeeStructure
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Fee { get; set; }

        public int ScholarshipCurrencyId { get; set; }

        public ScholarshipCurrency ScholarshipCurrency { get; set; }

        public int ScholarshipDurationId { get; set; }

        public ScholarshipDuration ScholarshipDuration { get; set; }

        public int ScholarshipEducationLevelId { get; set; }

        public ScholarshipEducationLevel ScholarshipEducationLevel { get; set; }

        public int ScholarshipCourseId { get; set; }

        public ScholarshipCourse ScholarshipCourse { get; set; }

        public int ScholarshipSchoolId { get; set; }

        public ScholarshipSchool ScholarshipSchool { get; set; }
    }
}
