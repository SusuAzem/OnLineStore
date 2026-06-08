using Core;
using Data.IRepository;

namespace Data.Repository
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        private readonly AppDbContext _db;

        public PaymentRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
