using Core;

namespace Data.IRepository
{
    public interface IShoppingCartLineRepository : IRepository<ShoppingCartLine>
    {
        int IncrementCount(ShoppingCartLine shoppingCart, int count);
        int DecrementCount(ShoppingCartLine shoppingCart, int count);
        IEnumerable<ShoppingCartLine> GetList(string id);
    }
}
