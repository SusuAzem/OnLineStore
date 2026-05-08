using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnLineStore.ViewModels
{

    /// <summary>
    /// For the product create & edit page
    /// </summary>
    public class ProductViewModel
    {
        public ProductItemViewModel? Product { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? ProductTypeList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? SpecialTagList { get; set; }
    }
}
