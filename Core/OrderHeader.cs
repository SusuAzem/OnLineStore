
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core
{
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string? UserNameIdentifier{ get; set; }
        [ForeignKey("UserNameIdentifier")]
        public virtual User? User { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        public DateTime ShippingDate { get; set; }

        public decimal OrderTotal { get; set; }

        public string? OrderStatus { get; set; }

        public string? TrackingNumber { get; set; }

        public string? Carrier { get; set; }

        public virtual IEnumerable<OrderItem>? OrderItems { get; set; } = [];

        public int PaymentId { get; set; }
        public virtual Payment? Payment { get; set; }
    }
}
