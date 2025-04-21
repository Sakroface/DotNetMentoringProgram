using System.Collections.Generic;
using System.Threading.Tasks;
using TicketingSystemBLL.Objects;

namespace TicketingSystemBLL.Interfaces
{
    /// <summary>
    /// Service to handle venue related business logic.
    /// </summary>
    public interface IVenueService
    {
        /// <summary>
        /// Method to create venues.
        /// </summary>
        /// <param name="obj">Venue object that represents the venue.</param>
        /// <returns>True - if add operation was completed successfully. False - otherwise.</returns>
        bool CreateVenue(Venue venue);

        /// <summary>
        /// Method to get all venues.
        /// </summary>
        /// <returns>Venues collection.</returns>
        Task<IEnumerable<Venue>> GetAllVenuesAsync();
    }
}
