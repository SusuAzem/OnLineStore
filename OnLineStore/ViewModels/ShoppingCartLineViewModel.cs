using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Core;

namespace OnLineStore.ViewModels
{
    public class ShoppingCartLineViewModel
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public ProductItemViewModel Product { get; set; }

        [Display(Name = "الكمية")]
        [Range(1, 1000, ErrorMessage = "الرجاء إدخال رقم بين 1 و 1000")]
        public int Count { get; set; }

        public string? UserNameIdentifier { get; set; }
        [ValidateNever]
        public virtual User? User { get; set; }


        [Display(Name = "السعر")]
        public float LinePrice { get; set; }
    }
}
