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
        private readonly DbSet<VenueSection> _sectionsDbSet;
        private readonly DbSet<VenueRow> _rowsDbSet;
        private readonly DbSet<VenueSeat> _seatsDbSet;
        private readonly DbSet<EventVenue> _eventVenuesDbSet;

        public VenueRepository(TicketingSystemDbContext context) : base(context) 
        {
            _sectionsDbSet = context.Set<VenueSection>();
            _rowsDbSet = context.Set<VenueRow>();
            _seatsDbSet = context.Set<VenueSeat>();
            _eventVenuesDbSet = context.Set<EventVenue>();
        }

        public override async Task<IEnumerable<Venue>> GetAllAsync()
        {
            return await DbSet
                    .Include(v => v.VenueSections)
                    .Include(v => v.VenueType)
                    .ToListAsync();

        }

        public async Task<Venue> GetVenueWithSectionsAsync(int venueId)
        {
            return await DbSet
                .Include(v => v.VenueSections)
                .FirstOrDefaultAsync(v => v.Id == venueId);
        }

        public async Task<VenueSection> GetSectionWithRowsAsync(int sectionId)
        {
            return await _sectionsDbSet
                .Include(v => v.VenueRows)
                .FirstOrDefaultAsync(v => v.Id == sectionId);
        }

        public async Task<VenueRow> GetRowWithSeatsAsync(int rowId)
        {
            return await _rowsDbSet
                .Include(v => v.VenueSeats)
                .FirstOrDefaultAsync(v => v.Id == rowId);
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

        public async Task<IEnumerable<Venue>> GetVenuesByEventAsync(int eventId)
        {
            var venueIds = await _eventVenuesDbSet.Where(ev => ev.EventId == eventId).Select(ev => ev.VenueId).ToListAsync();
            return await DbSet.Where(v => venueIds.Contains(v.Id)).ToListAsync();
        }
    }

}
