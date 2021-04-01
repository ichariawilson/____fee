using System.ComponentModel.DataAnnotations;

namespace Microsoft.Fee.Services.Student.Identity.API.Models
{
    public enum Period
    {
        [Display(Name = "One Term")]
        OneTerm,
        [Display(Name = "Two Terms")]
        TwoTerms,
        [Display(Name = "One Semester")]
        OneSemester,
        [Display(Name = "One Term")]
        TwoSemesters,
        [Display(Name = "Two Semesters")]
        ThreeSemesters,
        [Display(Name = "One Year")]
        OneYear,
        [Display(Name = "Two Years")]
        TwoYears,
        [Display(Name = "Three Years")]
        ThreeYears,
        [Display(Name = "Four Years")]
        FourYears,
        [Display(Name = "Five Years")]
        FiveYears,
        [Display(Name = "Six Years")]
        SixYears,
        [Display(Name = "Seven Years")]
        SevenYears,
        [Display(Name = "Eight Years")]
        EightYears
    }
}
