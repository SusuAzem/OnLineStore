using Core;

using Data;
using Data.IRepository;

namespace Data.Repository
{
    public class ProductTypeRepository : Repository<ProductType>, IProductTypeRepository
    {
        private readonly AppDbContext _db;

        public ProductTypeRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
    }
}