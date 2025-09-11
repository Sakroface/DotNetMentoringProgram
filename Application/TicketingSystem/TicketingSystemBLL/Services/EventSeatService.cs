using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystemBLL.DTO;
using TicketingSystemBLL.Enums;
using TicketingSystemBLL.Services.Interfaces;
using TicketingSystemDAL.UnitOfWork;

namespace TicketingSystemBLL.Services
{
    public class EventSeatService : IEventSeatService
    {
        private readonly ILogger<EventService> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public EventSeatService(ILogger<EventService> logger, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<EventSeatDto> GetEventSeatAsync(int seatId)
        {
            try
            {
                var entity = await _unitOfWork.EventSeatRepository.GetByIdAsync(seatId).ConfigureAwait(false);
                if (entity is null)
                {
                    return null;
                }

                var dto = _mapper.Map<EventSeatDto>(entity);

                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                throw;
            }
        }

        public async Task UpdateEventSeatsStatusAsync(IEnumerable<int> eventSeats, EventSeatStatus status)
        {
            try
            {
                if (eventSeats.Any(es => es <= 0))
                {
                    throw new ArgumentNullException("EventSeatService.UpdateEventSeatsStatusAsync. One of the provided Ids is less then or 0.");
                }

                _unitOfWork.BeginTransaction();

                var eventSeatEntities = await _unitOfWork.EventSeatRepository.GetByIdsAsync(eventSeats).ConfigureAwait(false);

                foreach (var entity in eventSeatEntities)
                {
                    entity.StatusId = (int)status;
                }

                _unitOfWork.EventSeatRepository.UpdateRange(eventSeatEntities);

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
