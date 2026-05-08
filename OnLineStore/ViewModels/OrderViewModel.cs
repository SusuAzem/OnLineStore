using Core;

namespace OnLineStore.ViewModels
{
    public class OrderViewModel
    {
        public OrderHeader? OrderHeader { get; set; }
        public IEnumerable<OrderItem>? OrderItems { get; set; }
    }
}
