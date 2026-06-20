using Core;

namespace OnLineStore.ViewModels
{
    public class ShoppingCartViewModel
    {
        public OrderHeaderViewModel? OrderHeader { get; set; }
        public IEnumerable<ShoppingCartLineViewModel>? ListCart { get; set; }

}
}
