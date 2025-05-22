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
using TicketingSystemBLL.Interfaces;

namespace TicketingSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VenueController : ControllerBase
    {
        private readonly ILogger<VenueController> _logger;
        private readonly IMapper _mapper;
        private readonly IVenueService _venueService;

        public VenueController(ILogger<VenueController> logger, IMapper mapper, IVenueService venueService)
        {
            _logger = logger;
            _venueService = venueService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VenueModel>>> GetAllVenuesAsync()
        {
            try
            {
                var venueDtos = await _venueService.GetAllVenuesAsync().ConfigureAwait(false);
                var venues = venueDtos.Select(e => _mapper.Map<VenueModel>(e));

                return Ok(venues);
            }
            catch (Exception ex)
            {
                _logger.LogError($"VenueController.GetAllVenuesAsync. Error: {ex.Message}.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while retrieving venues." });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VenueModel>> GetVenueAsync(int id)
        {
            try
            {
                var venueModel = await _venueService.GetVenueByIdAsync(id);

                if (venueModel == null)
                {
                    return NotFound();
                }

                return Ok(venueModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"VenueController.GetVenueAsync. Error: {ex.Message}.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while retrieving venue." });
            }
        }

        [HttpGet("{id}/sections")]
        public async Task<ActionResult<VenueModel>> GetVenueWithSectionsAsync(int id)
        {
            try
            {
                var venueModel = await _venueService.GetVenueWithSectionsAsync(id);

                if (venueModel == null)
                {
                    return NotFound();
                }

                return Ok(venueModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"VenueController.GetVenueWithSectionsAsync. Error: {ex.Message}.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while retrieving venue." });
            }
        }

        [HttpGet("/sections/{sectionId}/rows")]
        public async Task<ActionResult<VenueSectionModel>> GetVenueSectionAsync(int sectionId)
        {
            try
            {
                if (sectionId <= 0)
                {
                    return BadRequest("Section Id cannot be 0 or less.");
                }

                var venueModel = await _venueService.GetSectionWithRowsAsync(sectionId);

                if (venueModel == null)
                {
                    return NotFound();
                }

                return Ok(venueModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"VenueController.GetVenueSectionAsync. Error: {ex.Message}.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while retrieving venue." });
            }
        }

        [HttpGet("/rows/{rowId}/seats")]
        public async Task<ActionResult<VenueRowDto>> GetVenueRowAsync(int rowId)
        {
            try
            {
                if (rowId <= 0)
                {
                    return BadRequest("Section Id cannot be 0 or less.");
                }

                var venueModel = await _venueService.GetRowWithSeatsAsync(rowId);

                if (venueModel == null)
                {
                    return NotFound();
                }

                return Ok(venueModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"VenueController.GetVenueWithSectionsAsync. Error: {ex.Message}.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while retrieving venue." });
            }
        }

        [HttpPost]
        public async Task<ActionResult<VenueModel>> CreateVenueAsync(VenueModel venueModel)
        {
            try
            {
                if (string.IsNullOrEmpty(venueModel.Name))
                {
                    ModelState.AddModelError("Name", "Name cannot be an empty string.");
                    return BadRequest(ModelState);
                }

                if (venueModel.TypeId <= 0)
                {
                    ModelState.AddModelError("TypeId", "Type should be provided for the venue.");
                    return BadRequest(ModelState);
                }

                var dto = _mapper.Map<VenueDto>(venueModel);

                var createdVenueId = await _venueService.CreateVenueAsync(dto);

                return CreatedAtAction(nameof(CreateVenueAsync), new { id = createdVenueId });
            }
            catch (Exception ex)
            {
                _logger.LogError($"VenueController.CreateVenueAsync. Error: {ex.Message}.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while creating venue." });
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEvent(VenueModel venueModel)
        {
            try
            {
                if (string.IsNullOrEmpty(venueModel.Name))
                {
                    ModelState.AddModelError("Name", "Name cannot be an empty string.");
                    return BadRequest(ModelState);
                }

                if (venueModel.TypeId <= 0)
                {
                    ModelState.AddModelError("TypeId", "Type should be provided for the venue.");
                    return BadRequest(ModelState);
                }

                var dto = _mapper.Map<VenueDto>(venueModel);

                _venueService.UpdateVenue(dto);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"VenueController.UpdateEvent. Error: {ex.Message}.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while updating venue." });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteVenue(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException($"Id: {id} cannot be 0 or less.");
                }

                _venueService.DeleteVenue(id);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"VenueController.DeleteVenue. Error: {ex.Message}.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while deleting venue." });
            }
        }
    }
}