using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public EventController(ILogger<EventController> logger, IMapper mapper, IEventService eventService)
        {
            _logger = logger;
            _eventService = eventService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventModel>>> GetAllEventsAsync()
        {
            try
            {
                var eventDtos = await _eventService.GetAllEventsAsync().ConfigureAwait(false);
                var events = eventDtos.Select(e => _mapper.Map<EventModel>(e));

                return Ok(events);
            }
            catch (Exception ex)
            {
                _logger.LogError($"EventController.GetAllEventsAsync. Error: {ex.Message}.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while retrieving events." });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventModel>> GetEventAsync(int id)
        {
            try
            { 
                var eventModel = await _eventService.GetEventByIdAsync(id);

                if (eventModel == null)
                {
                    return NotFound();
                }

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

                var createdEventId = await _eventService.CreateEventAsync(dto);

                return CreatedAtAction(nameof(CreateEventAsync), new { id = createdEventId });
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

    }
}
