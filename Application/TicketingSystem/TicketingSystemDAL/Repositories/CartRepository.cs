using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TicketingSystemDAL.Entities;
using TicketingSystemDAL.EntityFramework;
using TicketingSystemDAL.Repositories.Interfaces;

namespace TicketingSystemDAL.Repositories
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(TicketingSystemDbContext context) : base(context)
        { }

        public async Task<Cart> GetCartWithSeatsAsync(Guid id)
        {
            return await DbSet.Include(e => e.EventSeats)
                              .ThenInclude(es => es.Price)
                              .Include(e => e.Status)
                              .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Cart> GetActiveCartByUserId(Guid userId)
        {
            return await DbSet.Include(e => e.EventSeats)
                              .Include("EventSeats.Price")
                              .Include(e => e.Status)
                              .FirstOrDefaultAsync(e => e.UserId == userId && e.StatusId == 1);
        }
    }
}
