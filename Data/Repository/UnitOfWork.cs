using Data;
using Data.IRepository;


namespace Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;

        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            ProductType = new ProductTypeRepository(_db);
            Product = new ProductRepository(_db);
            ShoppingCartLine = new ShoppingCartLineRepository(_db);
            OrderItem = new OrderItemRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            User = new UserRepository(_db);
            Message = new MessageRepository(_db);
            Payment = new PaymentRepository(_db);
        }
    
        public IProductTypeRepository ProductType { get; private set; }
        public IProductRepository Product { get; private set; }
        public IShoppingCartLineRepository ShoppingCartLine { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderItemRepository OrderItem { get; private set; }
        public IUserRepository User { get; private set; }
        public IPaymentRepository Payment { get; private set; }
        public IMessageRepository Message { get; set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }        
    }
}
