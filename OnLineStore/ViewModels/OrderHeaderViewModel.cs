
using OnLineStore.ViewModels;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core
{
    public class OrderHeaderViewModel
    {
        public int Id { get; set; }
        public string? UserNameIdentifier{ get; set; }
        public UserViewModel? User { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        public DateTime ShippingDate { get; set; }

        public decimal OrderTotal { get; set; }

        public string? OrderStatus { get; set; }

        public string? TrackingNumber { get; set; }

        public string? Carrier { get; set; }

        public virtual IEnumerable<OrderItemViewModel>? OrderItems { get; set; } = [];

        public int PaymentId { get; set; }
        public PaymentViewModel? Payment { get; set; }
    }
}
