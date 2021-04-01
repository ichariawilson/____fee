using System.ComponentModel.DataAnnotations;

namespace Microsoft.Fee.Services.Scholarship.API.ViewModel
{
    public class ScholarshipItemViewModel : EditImageViewModel
    {
        [Display(Name = "Web URL")]
        public string WebURL { get; set; }
        public int LocationId { get; set; }
        public string UserId { get; set; }
    }
}
