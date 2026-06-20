using Core;

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

using System.ComponentModel.DataAnnotations;

namespace OnLineStore.ViewModels
{
    public class ProductItemViewModel    
    {        
        public int Id { get; set; }

        [Required(ErrorMessage ="حقل الاسم مطلوب")]
        [Display(Name = "الاسم")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "حقل السعر مطلوب")] 
        [Display(Name = "السعر")]
        public decimal Price { get; set; }

        [ValidateNever]
        [Display(Name = "الصورة")]
        public string? Image { get; set; } = "/images/noimage.PNG";

        [Required(ErrorMessage = "حقل اللون مطلوب")]
        [Display(Name = "اللون")]
        public string? ProductColor { get; set; }

        [Required(ErrorMessage = "حقل توفر المنتج مطلوب")]
        [Display(Name = "متوفر؟")]
        public bool IsAvailable { get; set; }

        [Required(ErrorMessage = "حقل عرض المنتج مطلوب")]
        [Display(Name = "عرض المنتج؟")]
        public bool IsPublished { get; set; }

        [Required(ErrorMessage ="حقل نوع المنتج مطلوب")]
        [Display(Name = "نوع المنتج")]
        public int TypeId { get; set; }
        public ProductTypeViewModel? ProductType { get; set; }

    }
}
