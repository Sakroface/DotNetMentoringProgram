using System.Collections.Generic;
using System.Threading.Tasks;
using TicketingSystemDAL.Entities;

namespace TicketingSystemDAL.Repositories.Interfaces
{
    /// <summary>
    /// Venue repository.
    /// </summary>
    public interface IVenueRepository : IRepository<Venue>
    {
        /// <summary>
        /// Method to get the venue with sections.
        /// </summary>
        /// <param name="venueId">Id of the venue.</param>
        /// <returns>Venue with the sections.</returns>
        Task<Venue> GetVenueWithSectionsAsync(int venueId);

        /// <summary>
        /// Method to get available seats for the venue.
        /// </summary>
        /// <param name="eventId">Id of the event.</param>
        /// <param name="venueId">Id of the venue.</param>
        /// <returns>List with the available seats for the event.</returns>
        Task<IEnumerable<VenueSeat>> GetAvailableSeatsForEventAsync(int eventId, int venueId);

        /// <summary>
        /// Method to retrieve section with rows from the Db.
        /// </summary>
        /// <param name="sectionId">Unique identifier for the section.</param>
        /// <returns>Section with the related rows.</returns>
        Task<VenueSection> GetSectionWithRowsAsync(int sectionId);

        /// <summary>
        /// Method to get row with the seats.
        /// </summary>
        /// <param name="rowId">Identifier for the row.</param>
        /// <returns>Specific row with the seats that belong to it.</returns>
        Task<VenueRow> GetRowWithSeatsAsync(int rowId);

        /// <summary>
        /// Method to get venues by eventId.
        /// </summary>
        /// <param name="eventId">Id of the event that venues belong to.</param>
        /// <returns>Enumeration with the venues.</returns>
        Task<IEnumerable<Venue>> GetVenuesByEventAsync(int eventId);
    }

}
