using Core;

namespace OnLineStore.ViewModels
{
    public class ShoppingCartViewModel
    {
        public OrderHeader? OrderHeader { get; set; }
        public IEnumerable<ShoppingCartLine>? ListCart { get; set; }

}
}
