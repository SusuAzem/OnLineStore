
namespace Data.IRepository
{
    public interface IUnitOfWork
    {
        IProductTypeRepository ProductType { get; }
        IProductRepository Product { get; }
        IShoppingCartLineRepository ShoppingCartLine { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IOrderItemRepository OrderItem { get; }
        IUserRepository User { get; }
        IPaymentRepository Payment { get; }
        IMessageRepository Message { get; set; }

        void Dispose();
        Task Save();
    }
}
