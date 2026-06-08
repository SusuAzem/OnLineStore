using Core;

using Data;
using Data.IRepository;

namespace Data.Repository
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        private readonly AppDbContext _db;

        public OrderItemRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }       
    }
}
