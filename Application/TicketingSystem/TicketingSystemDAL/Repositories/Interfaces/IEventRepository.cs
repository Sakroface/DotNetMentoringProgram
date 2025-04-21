using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketingSystemDAL.Entities;

namespace TicketingSystemDAL.Repositories.Interfaces
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<IEnumerable<Event>> GetUpcomingEvents(DateTime fromDate);
        Task<IEnumerable<Event>> GetEventsByVenueAsync(int venueId);
    }
}
