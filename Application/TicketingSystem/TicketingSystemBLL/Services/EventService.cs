using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystemBLL.DTO;
using TicketingSystemBLL.Services.Interfaces;
using TicketingSystemDAL.Entities;
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

        public async Task ApproveEventAsync(int eventId)
        {
            try
            {
                if (eventId <= 0)
                {
                    throw new ArgumentNullException("EventService.ApproveEventAsync. Event Id cannot be 0 or less.");
                }

                await UpdateEventStatusAsync(eventId, Enums.EventStatus.Approved);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _unitOfWork.RollbackTransaction();

                throw;
            }
        }

        public async Task CancelEventAsync(int eventId)
        {
            try
            {
                if (eventId <= 0)
                {
                    throw new ArgumentNullException("EventService.CancelEventAsync. Event Id cannot be 0 or less.");
                }

                await UpdateEventStatusAsync(eventId, Enums.EventStatus.Cancelled);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _unitOfWork.RollbackTransaction();

                throw;
            }
        }

        public async Task<int> CreateEventAsync(EventDto dto)
        {
            try
            {
                if (dto is null)
                {
                    throw new ArgumentNullException("EventService.CreateEventAsync. Event object was null.");
                }

                _unitOfWork.BeginTransaction();

                var entity = _mapper.Map<Event>(dto);

                await _unitOfWork.EventRepository.InsertAsync(entity).ConfigureAwait(false);

                _unitOfWork.CommitTransaction();

                return entity.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _unitOfWork.RollbackTransaction();

                throw;
            }
        }

        public void DeleteEvent(int eventId)
        {
            try
            {
                if (eventId <= 0)
                {
                    throw new ArgumentNullException("EventService.DeleteEvent. Event Id cannot be 0 or less.");
                }

                _unitOfWork.BeginTransaction();

                _unitOfWork.EventRepository.Delete(eventId);

                _unitOfWork.CommitTransaction();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _unitOfWork.RollbackTransaction();

                throw;
            }
        }

        public async Task<IEnumerable<EventDto>> GetAllEventsAsync()
        {
            try
            {
                var result = await _unitOfWork.EventRepository.GetAllAsync().ConfigureAwait(false);
                if (result != null && result.Any())
                {
                    var mappedResult = result.Select(x => _mapper.Map<EventDto>(x)).ToList();
                    return mappedResult;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"EventService.GetAllEventsAsync. Error: {ex.Message}.");

                throw;
            }
            
            return Enumerable.Empty<EventDto>(); 
        }

        public async Task<EventDto> GetEventByIdAsync(int eventId)
        {
            try
            {
                if (eventId <= 0)
                {
                    throw new ArgumentNullException("EventService.GetEventByIdAsync. Event Id cannot be 0 or less.");
                }

                var entity = await _unitOfWork.EventRepository.GetByIdAsync(eventId).ConfigureAwait(false);
                if (entity is null)
                {
                    return null;
                }

                return _mapper.Map<EventDto>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"EventService.GetEventByIdAsync. Error: {ex.Message}.");

                throw;
            }
        }

        public void UpdateEvent(EventDto dto)
        {
            try
            {
                if (dto is null)
                {
                    throw new ArgumentNullException("EventService.UpdateEventAsync. Event object was null.");
                }

                _unitOfWork.BeginTransaction();

                var entity = _mapper.Map<Event>(dto);

                _unitOfWork.EventRepository.Update(entity);

                _unitOfWork.CommitTransaction();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _unitOfWork.RollbackTransaction();

                throw;
            }
        }

        public async Task UpdateEventStatusAsync(int eventId, Enums.EventStatus eventStatus)
        {
            try
            {
                if (eventId <= 0)
                {
                    throw new ArgumentNullException("EventService.UpdateEventStatusAsync. Event Id cannot be 0 or less.");
                }

                _unitOfWork.BeginTransaction();

                var entity = await _unitOfWork.EventRepository.GetByIdAsync(eventId);
                entity.StatusId = (int)eventStatus;

                _unitOfWork.EventRepository.Update(entity);

                _unitOfWork.CommitTransaction();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _unitOfWork.RollbackTransaction();

                throw;
            }
        }
    }
}
