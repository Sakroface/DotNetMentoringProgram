using System;
using TicketingSystemBLL.Services.Interfaces;

namespace TicketingSystemBLL.Services
{
    public class UtilityService : IUtilityService
    {
        public bool IsValidGuid(string guid)
        {
            return Guid.TryParse(guid, out _);
        }
    }
}
