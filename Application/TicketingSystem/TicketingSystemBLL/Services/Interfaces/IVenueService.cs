using System.Collections.Generic;
using System.Threading.Tasks;
using TicketingSystemBLL.DTO;

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
        /// <param name="venue">Venue object that represents the venue.</param>
        /// <returns>Id of the inserted entity.</returns>
        Task<int> CreateVenueAsync(VenueDto venue);

        /// <summary>
        /// Method to get all venues.
        /// </summary>
        /// <returns>Venues collection.</returns>
        Task<IEnumerable<VenueDto>> GetAllVenuesAsync();

        /// <summary>
        /// Method to delete venue by it's Id.
        /// </summary>
        /// <param name="venueId">Id of the venue to delete.</param>
        void DeleteVenue(int venueId);

        /// <summary>
        /// Method to get venue by it's Id.
        /// </summary>
        /// <param name="venueId">Id of the venue.</param>
        /// <returns>Venue dto with the data.</returns>
        Task<VenueDto> GetVenueByIdAsync(int venueId);

        /// <summary>
        /// Method to update the venue.
        /// </summary>
        /// <param name="dto">Dto of the venue.</param>
        void UpdateVenue(VenueDto dto);

        /// <summary>
        /// Gets venue with all configurations for the seats.
        /// </summary>
        /// <param name="venueId">Id of the venue.</param>
        /// <returns>All sections with rows and seats.</returns>
        Task<VenueDto> GetVenueWithSectionsAsync(int venueId);

        /// <summary>
        /// Method to get section with rows.
        /// </summary>
        /// <param name="sectionId">Id of the section.</param>
        /// <returns>Section with the related rows.</returns>
        Task<VenueSectionDto> GetSectionWithRowsAsync(int sectionId);

        /// <summary>
        /// Method to get row with the related seats.
        /// </summary>
        /// <param name="rowId">Id of the row.</param>
        /// <returns>Row with the related seats.</returns>
        Task<VenueRowDto> GetRowWithSeatsAsync(int rowId);
    }
}
