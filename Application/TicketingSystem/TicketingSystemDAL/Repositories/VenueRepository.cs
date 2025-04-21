using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystemDAL.Entities;
using TicketingSystemDAL.EntityFramework;
using TicketingSystemDAL.Repositories.Interfaces;

namespace TicketingSystemDAL.Repositories
{
    public class VenueRepository : Repository<Venue>, IVenueRepository
    {
        public VenueRepository(TicketingSystemDbContext context) : base(context) { }

        public async Task<Venue> GetVenueWithSectionsAsync(int venueId)
        {
            return await DbSet
                .Include(v => v.VenueSections.Select(s => s.VenueRows.Select(r => r.VenueSeats)))
                .FirstOrDefaultAsync(v => v.Id == venueId);
        }

        public async Task<IEnumerable<VenueSeat>> GetAvailableSeatsForEventAsync(int eventId, int venueId)
        {
            var allSeats = await Context.VenueSeats
                .Where(s => s.Row.Section.VenueId == venueId)
                .ToListAsync();

            var bookedSeatIds = await Context.EventSeats
                .Where(es => es.EventId == eventId)
                .Select(es => es.SeatId)
                .ToListAsync();

            return allSeats.Where(s => !bookedSeatIds.Contains(s.Id));
        }
    }

}
