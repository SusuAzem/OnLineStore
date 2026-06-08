using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Core
{
    public class ShoppingCartLine
    {
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }

        [Range(1, 1000)]
        public int Count { get; set; }

        public decimal LinePrice { get; set; }

        [ForeignKey("User")]
        public string? UserNameIdentifier { get; set; }
        [ForeignKey("UserNameIdentifier")]
        public virtual User? User { get; set; }
    }
}
