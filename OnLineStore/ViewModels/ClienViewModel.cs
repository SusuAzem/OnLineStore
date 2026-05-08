using System.ComponentModel.DataAnnotations;

namespace OnLineStore.ViewModels
{
    public class ClientViewModel
    {

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
        public int PostalCode { get; set; }
    }
}
