using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystem.Models;
using TicketingSystemBLL.Services.Interfaces;

namespace TicketingSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        private readonly ILogger<EventController> _logger;
        private readonly IMapper _mapper;
        private readonly IEventService _eventService;

        public EventController(ILogger<EventController> logger, IMapper mapper, IEventService eventService)
        {
            _logger = logger;
            _eventService = eventService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<EventModel>> GetAllEventsAsync()
        {
            try
            {
                var events = await _eventService.GetAllEventsAsync().ConfigureAwait(false);
                
                return events.Select(e => _mapper.Map<EventModel>(e));
            }
            catch (Exception ex)
            {
                _logger.LogError($"EventController.GetAllEventsAsync. Error: {ex.Message}.");
                return Enumerable.Empty<EventModel>();
            }
        }
    }
}
