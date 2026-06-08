using Newtonsoft.Json;

using System.ComponentModel.DataAnnotations;

namespace Core
{
    public class ProductType
    {
        public int Id { get; set; }

        [Required]
        public string? Type { get; set; }

        [JsonIgnore] 
        public virtual IEnumerable<Product>? Products { get; set; }

    }
}
