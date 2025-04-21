using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystemBLL.Objects;
using TicketingSystemBLL.Services.Interfaces;
using TicketingSystemDAL.UnitOfWork;

namespace TicketingSystemBLL.Services
{
    /// <summary>
    /// Basic service to handle event related operations.
    /// </summary>
    public class EventService : IEventService
    {
        private readonly ILogger<EventService> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public EventService(ILogger<EventService> logger, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public bool CreateEvent(Event obj)
        {
            var result = false;
            try
            {
                if (obj is null)
                {
                    throw new ArgumentNullException("EventService.CreateEvent. Event object was null.");
                }
                _unitOfWork.BeginTransaction();

                
                

                _unitOfWork.CommitTransaction();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _unitOfWork.RollbackTransaction();
            }

            return result;
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            try
            {
                var result = await _unitOfWork.EventRepository.GetAllAsync().ConfigureAwait(false);
                if (result != null && result.Any())
                {
                    return result.Select(x => _mapper.Map<Event>(x));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"EventService.GetAllEventsAsync. Error: {ex.Message}.");
            }
            
            return Enumerable.Empty<Event>(); 
        }
    }
}
