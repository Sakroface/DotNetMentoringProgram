using System;

namespace TicketingSystemBLL.Services.Interfaces
{
    public interface IUtilityService
    {
        /// <summary>
        /// Method to validate if guid is correct.
        /// </summary>
        /// <param name="guid">String with Guid to validate.</param>
        /// <returns>True - if guid is valid, otherwise - false.</returns>
        bool IsValidGuid(string guid);
    }
}
