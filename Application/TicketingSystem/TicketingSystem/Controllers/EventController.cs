using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystem.Models;
using TicketingSystemBLL.DTO;
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
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;
        private readonly string ALL_EVENTS_CACHE_KEY;
        private readonly string EVENT_CACHE_KEY_PREFIX;
        private readonly TimeSpan _cacheExpiration;

        public EventController(ILogger<EventController> logger, IMapper mapper, IEventService eventService, IMemoryCache cache, IConfiguration configuration)
        {
            _logger = logger;
            _eventService = eventService;
            _mapper = mapper;
            _cache = cache;
            _configuration = configuration;

            ALL_EVENTS_CACHE_KEY = _configuration.GetValue("ALL_EVENTS_CACHE_KEY", string.Empty);
            EVENT_CACHE_KEY_PREFIX = _configuration.GetValue("EVENT_CACHE_KEY_PREFIX", string.Empty);
            _cacheExpiration = TimeSpan.FromMinutes(30);
        }

        [HttpGet]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new string[] { "*" })]
        public async Task<ActionResult<IEnumerable<EventModel>>> GetAllEventsAsync()
        {
            Response.Headers.TryAdd("Cache-Control", "public, max-age=300, must-revalidate");
            Response.Headers.Add("Vary", "Accept, Accept-Encoding");

            try
            {
                /*
                ///In-memory caching is here. It is disabled for now for the user side management caching and cache controll by the Last-Modified and If-Modified-Since headers.
                if (_cache.TryGetValue(ALL_EVENTS_CACHE_KEY, out IEnumerable<EventModel> cachedEvents))
                {
                    _logger.LogInformation("Retrieved events from cache.");
                    return Ok(cachedEvents);
                }
                */

                var eventDtos = await _eventService.GetAllEventsAsync().ConfigureAwait(false);
                var events = eventDtos.Select(e => _mapper.Map<EventModel>(e));

                var lastModified = events.Any() ?
                    events.Max(e => e.LastModified) :
                    DateTime.UtcNow;

                // Check If-Modified-Since header
                if (Request.Headers.ContainsKey("If-Modified-Since") &&
                    DateTime.TryParse(Request.Headers["If-Modified-Since"], out var ifModifiedSince) &&
                    lastModified <= ifModifiedSince.AddSeconds(1)) // Add 1 second tolerance
                {
                    return StatusCode(StatusCodes.Status304NotModified); // Not Modified
                }

                _cache.Set(ALL_EVENTS_CACHE_KEY, events, _cacheExpiration);
                _logger.LogInformation("Added events to the cache.");

                Response.Headers.Add("Last-Modified", lastModified.ToString("R"));

                return Ok(events);
            }
            catch (Exception ex)
            {
                _logger.LogError($"EventController.GetAllEventsAsync. Error: {ex.Message}.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while retrieving events." });
            }
        }

        [HttpGet("{id}", Name = "GetEventAsync")]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new string[] { "id" })]
        public async Task<ActionResult<EventModel>> GetEventAsync(int id)
        {
            try
            {
                string cacheKey = $"{EVENT_CACHE_KEY_PREFIX}{id}";

                if (_cache.TryGetValue(cacheKey, out EventModel cachedEvent))
                {
                    _logger.LogInformation($"Retrieved event {id} from cache.");
                    return Ok(cachedEvent);
                }

                var eventDto = await _eventService.GetEventByIdAsync(id);

                if (eventDto == null)
                {
                    return NotFound();
                }

                var eventModel = _mapper.Map<EventModel>(eventDto);

                _cache.Set(cacheKey, eventModel, _cacheExpiration);
                _logger.LogInformation($"Cached event {id}");

                return Ok(eventModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"EventController.GetEventAsync. Error: {ex.Message}.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while retrieving event." });
            }
        }

        [HttpPost]
        public async Task<ActionResult<EventModel>> CreateEventAsync(EventModel eventModel)
        {
            try
            {
                if (eventModel.EndDate <= eventModel.StartDate)
                {
                    ModelState.AddModelError("EndDate", "End date must be after start date");
                    return BadRequest(ModelState);
                }

                if (eventModel.SetupTime < 0)
                {
                    ModelState.AddModelError("SetupTime", "Setup time cannot be negative");
                    return BadRequest(ModelState);
                }

                if (eventModel.TeardownTime < 0)
                {
                    ModelState.AddModelError("TeardownTime", "Teardown time cannot be negative");
                    return BadRequest(ModelState);
                }

                var dto = _mapper.Map<EventDto>(eventModel);
                dto.EventStatus = TicketingSystemBLL.Enums.EventStatus.Created;
                var createdEventId = await _eventService.CreateEventAsync(dto);

                //Invalidate server side caches
                RemoveEventCache();

                // Invalidate client caches
                Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");

                return StatusCode(StatusCodes.Status201Created, new { id = createdEventId });
            }
            catch (Exception ex)
            {
                _logger.LogError($"EventController.CreateEventAsync. Error: {ex.Message}.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while creating event." });
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEvent(EventModel eventModel)
        {
            try
            { 
                if (eventModel.EndDate <= eventModel.StartDate)
                {
                    ModelState.AddModelError("EndDate", "End date must be after start date");
                    return BadRequest(ModelState);
                }

                if (eventModel.SetupTime < 0)
                {
                    ModelState.AddModelError("SetupTime", "Setup time cannot be negative");
                    return BadRequest(ModelState);
                }

                if (eventModel.TeardownTime < 0)
                {
                    ModelState.AddModelError("TeardownTime", "Teardown time cannot be negative");
                    return BadRequest(ModelState);
                }

                var dto = _mapper.Map<EventDto>(eventModel);
                _eventService.UpdateEvent(dto);

                RemoveEventCache(dto.Id);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"EventController.UpdateEventAsync. Error: {ex.Message}.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while updating event." });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEvent(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException($"Id: {id} cannot be 0 or less.");
                }

                _eventService.DeleteEvent(id);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"EventController.DeleteEventAsync. Error: {ex.Message}.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while deleting event." });
            }
        }

        private void RemoveEventCache(int? eventId = null)
        {
            _cache.Remove(ALL_EVENTS_CACHE_KEY);
            _logger.LogInformation("Removed all events cache.");

            if (eventId.HasValue)
            {
                string specificCacheKey = $"{EVENT_CACHE_KEY_PREFIX}{eventId.Value}";
                _cache.Remove(specificCacheKey);
                _logger.LogInformation($"Removed cache for event {eventId.Value}.");
            }
        }

    }
}
