using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace OnLineStore.ViewModels
{
    public class UserViewModel
    {
        [Required]
        [Display(Name = " المعرف")]
        public string? Nameidentifier { get; set; }

        [Required(ErrorMessage = "حقل الاسم مطلوب")]
        [Display(Name = "الاسم")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "حقل اسم العائلة مطلوب")]
        [Display(Name = "اسم العائلة")]
        public string? Surname { get; set; }

        [Required(ErrorMessage = "حقل البريد الالكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "ليست صيغة صحيحة للبريد الاكتروني ")]
        [Display(Name = " البريد الالكتروني")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "حقل الرقم مطلوب")]
        [Display(Name = "الرقم")]
        [RegularExpression(@"^(09)(5|0|3|6|4|9|1|8|7)([0-9]{7})$", ErrorMessage = "الرقم يبدأ ب 09")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "حقل تاريخ الميلاد مطلوب")]
        [Display(Name = "تاريخ الميلاد")]
        public DateOnly DateOfBirth { get; set; }

        [ValidateNever]
        public DateTimeOffset? LockoutEnd { get; set; }

        [Required(ErrorMessage = "حقل الشارع مطلوب")]
        [Display(Name = "الشارع")]
        public string? StreetAddress { get; set; }

        [Required(ErrorMessage = "حقل الحي مطلوب")]
        [Display(Name = "الحي")]
        public string? Neighborhood { get; set; }

        [Required(ErrorMessage = "حقل المدينة مطلوب")]
        [Display(Name = "المدينة")]
        public string? City { get; set; }

        [Display(Name = "الرمز البريدي")]
        [Range(10000, 99999)]
        [DataType(DataType.PostalCode)]
        public string? PostalCode { get; set; }
    }
}
