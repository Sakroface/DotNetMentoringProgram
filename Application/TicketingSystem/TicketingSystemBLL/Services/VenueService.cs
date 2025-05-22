using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystemBLL.Interfaces;
using TicketingSystemBLL.DTO;
using TicketingSystemDAL.UnitOfWork;
using TicketingSystemDAL.Entities;

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

        public async Task<IEnumerable<VenueDto>> GetAllVenuesAsync()
        {
            try
            {
                var result = await _unitOfWork.VenueRepository.GetAllAsync().ConfigureAwait(false);
                if (result != null && result.Any())
                {
                    return result.Select(x => _mapper.Map<VenueDto>(x));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"VenueService.GetAllVenuesAsync. Error: {ex.Message}.");
            }

            return Enumerable.Empty<VenueDto>();
        }

        public async Task<int> CreateVenueAsync(VenueDto dto)
        {
            try
            {
                if (dto is null)
                {
                    throw new ArgumentNullException("VenueService.CreateVenueAsync. Venue object was null.");
                }

                _unitOfWork.BeginTransaction();

                var entity = _mapper.Map<Venue>(dto);

                await _unitOfWork.VenueRepository.InsertAsync(entity).ConfigureAwait(false);

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

        public void DeleteVenue(int venueId)
        {
            try
            {
                if (venueId <= 0)
                {
                    throw new ArgumentNullException("VenueService.DeleteVenue. Venue Id cannot be 0 or less.");
                }

                _unitOfWork.BeginTransaction();

                _unitOfWork.VenueRepository.Delete(venueId);

                _unitOfWork.CommitTransaction();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _unitOfWork.RollbackTransaction();

                throw;
            }
        }

        public async Task<VenueDto> GetVenueByIdAsync(int venueId)
        {
            try
            {
                if (venueId <= 0)
                {
                    throw new ArgumentNullException("VenueService.GetVenueByIdAsync. Venue Id cannot be 0 or less.");
                }

                var entity = await _unitOfWork.VenueRepository.GetByIdAsync(venueId).ConfigureAwait(false);
                if (entity is null)
                {
                    return null;
                }

                return _mapper.Map<VenueDto>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"VenueService.GetVenueByIdAsync. Error: {ex.Message}.");

                throw;
            }
        }

        public void UpdateVenue(VenueDto dto)
        {
            try
            {
                if (dto is null)
                {
                    throw new ArgumentNullException("VenueService.UpdateVenue. Venue object was null.");
                }

                _unitOfWork.BeginTransaction();

                var entity = _mapper.Map<Venue>(dto);

                _unitOfWork.VenueRepository.Update(entity);

                _unitOfWork.CommitTransaction();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _unitOfWork.RollbackTransaction();

                throw;
            }
        }

        public async Task<VenueDto> GetVenueWithSectionsAsync(int venueId)
        {
            try
            {
                if (venueId <= 0)
                {
                    throw new ArgumentNullException("VenueService.GetVenueWithConfigurations. Venue Id cannot be 0 or less.");
                }

                var entity = await _unitOfWork.VenueRepository.GetVenueWithSectionsAsync(venueId).ConfigureAwait(false);
                if (entity is null)
                {
                    return null;
                }

                return _mapper.Map<VenueDto>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"VenueService.GetVenueWithSections. Error: {ex.Message}.");

                throw;
            }
        }

        public async Task<VenueSectionDto> GetSectionWithRowsAsync(int sectionId)
        {
            try
            {
                if (sectionId <= 0)
                {
                    throw new ArgumentNullException("VenueService.GetVenueWithConfigurations. Section Id cannot be 0 or less.");
                }

                var entity = await _unitOfWork.VenueRepository.GetSectionWithRowsAsync(sectionId).ConfigureAwait(false);
                if (entity is null)
                {
                    return null;
                }

                return _mapper.Map<VenueSectionDto>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"VenueService.GetSectionWithRows. Error: {ex.Message}.");

                throw;
            }
        }

        public async Task<VenueRowDto> GetRowWithSeatsAsync(int rowId)
        {
            try
            {
                if (rowId <= 0)
                {
                    throw new ArgumentNullException("VenueService.GetRowWithSeats. Row Id cannot be 0 or less.");
                }

                var entity = await _unitOfWork.VenueRepository.GetRowWithSeatsAsync(rowId).ConfigureAwait(false);
                if (entity is null)
                {
                    return null;
                }

                var dto = _mapper.Map<VenueRowDto>(entity);
                dto.VenueSeats = dto.VenueSeats.OrderBy(d => d.Number);

                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"VenueService.GetRowWithSeats. Error: {ex.Message}.");

                throw;
            }
        }
    }
}
