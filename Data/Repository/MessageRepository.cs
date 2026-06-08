using Core;

using Data.IRepository;

namespace Data.Repository
{
    internal class MessageRepository : Repository<Message>, IMessageRepository
    {
        private readonly AppDbContext _db;

        public MessageRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
   
