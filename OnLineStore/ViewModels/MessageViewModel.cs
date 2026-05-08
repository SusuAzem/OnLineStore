using System.ComponentModel.DataAnnotations;

namespace OnLineStore.ViewModels
{
    public class MessageViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "حقل الاسم مطلوب")]
        [Display(Name = "الاسم")]
        public string Name { get; set; } = "";


        [Required(ErrorMessage = "حقل البريد مطلوب")]
        [EmailAddress(ErrorMessage = "صيغة عنوان بريد الكتروني غير صحيحة")]
        [Display(Name = "البريد الالكتروني")]
        public string Email { get; set; } ="";


        [Required(ErrorMessage = "حقل رقم التواصل مطلوب")]
        [RegularExpression(@"^(05)(5|0|3|6|4|9|1|8|7)([0-9]{7})$", ErrorMessage = "الرقم يبدأ ب 05")]
        [Display(Name = "رقم التواصل")]
        public string PhoneNumber { get; set; } = "";


        [Required(ErrorMessage = "حقل المحتوى مطلوب")]
        [Display(Name = "المحتوى")]
        public string Content { get; set; } = "";

        [Display(Name = "وقت الإرسال")]
        public DateTime TimeSend { get; set; } = DateTime.Now;
    }
}
