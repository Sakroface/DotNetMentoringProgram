using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystemDAL.Entities;
using TicketingSystemDAL.EntityFramework;
using TicketingSystemDAL.Repositories.Interfaces;

namespace TicketingSystemDAL.Repositories
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(TicketingSystemDbContext context) : base(context) { }

        public override async Task<Event> GetByIdAsync(object id)
        {
            return await DbSet.Include(e => e.Status)
                              .FirstOrDefaultAsync(e => e.Id == (int)id);
        }

        public async Task<IEnumerable<Event>> GetUpcomingEvents(DateTime fromDate)
        {
            return await DbSet.Where(e => e.StartDate >= fromDate).ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsByVenueAsync(int venueId)
        {
            return await DbSet.Where(e => e.EventVenues.Any(ev => ev.VenueId == venueId)).ToListAsync();
        }
    }
}
