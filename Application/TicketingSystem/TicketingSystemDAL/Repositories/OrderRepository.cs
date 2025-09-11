using TicketingSystemDAL.Entities;
using TicketingSystemDAL.EntityFramework;
using TicketingSystemDAL.Repositories.Interfaces;

namespace TicketingSystemDAL.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(TicketingSystemDbContext context) : base(context) 
        { }
    }
}
