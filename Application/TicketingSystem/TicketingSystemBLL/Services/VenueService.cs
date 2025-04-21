using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystemBLL.Interfaces;
using TicketingSystemBLL.Objects;
using TicketingSystemDAL.UnitOfWork;

namespace TicketingSystemBLL.Services
{
    public class VenueService : IVenueService
    {
        private readonly ILogger<VenueService> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public VenueService(ILogger<VenueService> logger, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public bool CreateVenue(Venue venue)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<Venue>> GetAllVenuesAsync()
        {
            try
            {
                var result = await _unitOfWork.VenueRepository.GetAllAsync().ConfigureAwait(false);
                if (result != null && result.Any())
                {
                    return result.Select(x => _mapper.Map<Venue>(x));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"EventService.GetAllEventsAsync. Error: {ex.Message}.");
            }

            return Enumerable.Empty<Venue>();
        }
    }
}
