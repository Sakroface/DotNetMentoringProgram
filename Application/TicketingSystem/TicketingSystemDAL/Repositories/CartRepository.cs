using TicketingSystemDAL.Entities;
using TicketingSystemDAL.EntityFramework;
using TicketingSystemDAL.Repositories.Interfaces;

namespace TicketingSystemDAL.Repositories
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(TicketingSystemDbContext context) : base(context)
        { }
    }
}
