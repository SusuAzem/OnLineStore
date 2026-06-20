using Core;

namespace OnLineStore.ViewModels
{
    public class OrderViewModel
    {
        public OrderHeaderViewModel? OrderHeader { get; set; }
        public UserViewModel? User { get; set; }
        public PaymentViewModel? Payment { get; set; }
        public IEnumerable<OrderItemViewModel>? OrderItems { get; set; }
    }
}
