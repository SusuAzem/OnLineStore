using Newtonsoft.Json;

using OnLineStore.ViewModels;

using System.ComponentModel.DataAnnotations;

namespace OnLineStore.ViewModels
{
    public class ProductTypeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "حقل النوع مطلوب")]
        [Display(Name = "النوع")]
        public string? Type { get; set; }

        //[JsonIgnore] 
        //public virtual IEnumerable<ProductItemViewModel>? Products { get; set; }

    }
}
