using System.Collections.Generic;
using System.Threading.Tasks;
using TicketingSystemDAL.Entities;

namespace TicketingSystemDAL.Repositories.Interfaces
{
    public interface IVenueRepository : IRepository<Venue>
    {
        Task<Venue> GetVenueWithSectionsAsync(int venueId);
        Task<IEnumerable<VenueSeat>> GetAvailableSeatsForEventAsync(int eventId, int venueId);
    }

}
