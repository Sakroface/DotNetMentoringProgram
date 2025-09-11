using TicketingSystemDAL.Entities;
using TicketingSystemDAL.EntityFramework;
using TicketingSystemDAL.Repositories.Interfaces;

namespace TicketingSystemDAL.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(TicketingSystemDbContext context) : base(context)
        { }
    }
}
