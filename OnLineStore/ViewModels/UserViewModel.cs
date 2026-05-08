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

        [Required(ErrorMessage = "حقل الاسم الأول مطلوب")]
        [Display(Name = "الاسم الأول")]
        public string? GivenName { get; set; }

        [Required(ErrorMessage = "حقل اسم العائلة مطلوب")]
        [Display(Name = "اسم العائلة")]
        public string? Surname { get; set; }

        [Required(ErrorMessage = "حقل البريد الالكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "ليست صيغة صحيحة للبريد الاكتروني ")]
        [Display(Name = " البريد الالكتروني")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "حقل الرقم مطلوب")]
        [Display(Name = "الرقم")]
        [RegularExpression(@"^(05)(5|0|3|6|4|9|1|8|7)([0-9]{7})$", ErrorMessage = "الرقم يبدأ ب 05")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "حقل العمر مطلوب")]
        [Display(Name = "العمر")]
        [Range(3, 100 , ErrorMessage ="العمر من 3 وما فوق")]
        public int Age { get; set; }

        [Display(Name = "الأدوار")]
        [ValidateNever]
        public IList<string> Roles { get; set; } = new List<string>();

        [ValidateNever]
        public bool Registered { get; set; }

        [ValidateNever]
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}
