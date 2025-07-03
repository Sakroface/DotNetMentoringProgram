using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TicketingSystemDAL.Entities;
using TicketingSystemDAL.EntityFramework;
using TicketingSystemDAL.Repositories.Interfaces;

namespace TicketingSystemDAL.Repositories
{
    class EventSeatRepository : Repository<EventSeat>, IEventSeatRepository
    {
        public EventSeatRepository(TicketingSystemDbContext context) : base(context)
        { }

        ///<inheritdoc/>
        public override async Task<EventSeat> GetByIdAsync(object id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id), "ID cannot be null.");

            return await DbSet.Include(es => es.Price)
                              .FirstOrDefaultAsync(es => es.Id == Convert.ToInt32(id));
        }
    }
}
