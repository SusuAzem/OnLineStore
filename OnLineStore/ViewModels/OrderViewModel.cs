using Core;

namespace OnLineStore.ViewModels
{
    public class OrderViewModel
    {
        public OrderHeader? OrderHeader { get; set; }
        public User? User { get; set; }
        public Payment? Payment { get; set; }
        public IEnumerable<OrderItem>? OrderItems { get; set; }
    }
}
