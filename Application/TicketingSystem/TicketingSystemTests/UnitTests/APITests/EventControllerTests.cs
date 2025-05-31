using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystem.Controllers;
using TicketingSystem.Models;
using TicketingSystemBLL.DTO;
using TicketingSystemBLL.Services.Interfaces;
using Xunit;

namespace TicketingSystemTests.APITests
{
    public class EventControllerTests
    {
        private readonly Mock<ILogger<EventController>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IEventService> _eventServiceMock;
        private readonly EventController _controller;

        public EventControllerTests()
        {
            _loggerMock = new Mock<ILogger<EventController>>();
            _mapperMock = new Mock<IMapper>();
            _eventServiceMock = new Mock<IEventService>();
            _controller = new EventController(_loggerMock.Object, _mapperMock.Object, _eventServiceMock.Object);
        }

        #region GetAllEventsAsync Tests

        [Fact]
        public async Task GetAllEventsAsync_ReturnsOkResult_WithListOfEvents()
        {
            // Arrange
            var eventDtos = new List<EventDto>
            {
                new EventDto { Id = 1, Name = "Event 1" },
                new EventDto { Id = 2, Name = "Event 2" }
            };

            var eventModels = new List<EventModel>
            {
                new EventModel { Id = 1, Name = "Event 1" },
                new EventModel { Id = 2, Name = "Event 2" }
            };

            _eventServiceMock.Setup(s => s.GetAllEventsAsync())
                .ReturnsAsync(eventDtos);

            _mapperMock.Setup(m => m.Map<EventModel>(It.IsAny<EventDto>()))
                .Returns<EventDto>(dto => eventModels.FirstOrDefault(m => m.Id == dto.Id));

            // Act
            var result = await _controller.GetAllEventsAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedEvents = Assert.IsAssignableFrom<IEnumerable<EventModel>>(okResult.Value);
            Assert.Equal(2, returnedEvents.Count());
        }

        [Fact]
        public async Task GetAllEventsAsync_WhenExceptionOccurs_ReturnsInternalServerError()
        {
            // Arrange
            _eventServiceMock.Setup(s => s.GetAllEventsAsync())
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetAllEventsAsync();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        #endregion

        #region GetEventAsync Tests

        [Fact]
        public async Task GetEventAsync_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var eventDto = new EventDto { Id = 1, Name = "Test Event" };
            _eventServiceMock.Setup(s => s.GetEventByIdAsync(1))
                .ReturnsAsync(eventDto);

            // Act
            var result = await _controller.GetEventAsync(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(eventDto, okResult.Value);
        }

        [Fact]
        public async Task GetEventAsync_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _eventServiceMock.Setup(s => s.GetEventByIdAsync(999))
                .ReturnsAsync((EventDto)null);

            // Act
            var result = await _controller.GetEventAsync(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetEventAsync_WhenExceptionOccurs_ReturnsInternalServerError()
        {
            // Arrange
            _eventServiceMock.Setup(s => s.GetEventByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetEventAsync(1);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        #endregion

        #region CreateEventAsync Tests

        [Fact]
        public async Task CreateEventAsync_WithValidModel_ReturnsCreatedAtAction()
        {
            // Arrange
            var eventModel = new EventModel
            {
                Id = 1,
                Name = "Test Event",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                SetupTime = 60,
                TeardownTime = 30
            };

            var eventDto = new EventDto
            {
                Id = 1,
                Name = "Test Event"
            };

            _mapperMock.Setup(m => m.Map<EventDto>(It.IsAny<EventModel>()))
                .Returns(eventDto);

            _eventServiceMock.Setup(s => s.CreateEventAsync(It.IsAny<EventDto>()))
                .ReturnsAsync(1);

            // Act
            var result = await _controller.CreateEventAsync(eventModel);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("CreateEventAsync", createdAtActionResult.ActionName);

            // Fluent assertion to validate the response result with id.
            createdAtActionResult.Value.Should().BeEquivalentTo(new { id = 1 });
        }

        [Fact]
        public async Task CreateEventAsync_WithInvalidDates_ReturnsBadRequest()
        {
            // Arrange
            var eventModel = new EventModel
            {
                Id = 1,
                Name = "Test Event",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(-1), // Invalid: end date before start date
                SetupTime = 60,
                TeardownTime = 30
            };

            // Act
            var result = await _controller.CreateEventAsync(eventModel);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var modelState = Assert.IsType<SerializableError>(badRequestResult.Value);
            Assert.True(modelState.ContainsKey("EndDate"));
        }

        [Fact]
        public async Task CreateEventAsync_WithNegativeSetupTime_ReturnsBadRequest()
        {
            // Arrange
            var eventModel = new EventModel
            {
                Id = 1,
                Name = "Test Event",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                SetupTime = -60, // Invalid: negative setup time
                TeardownTime = 30
            };

            // Act
            var result = await _controller.CreateEventAsync(eventModel);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var modelState = Assert.IsType<SerializableError>(badRequestResult.Value);
            Assert.True(modelState.ContainsKey("SetupTime"));
        }

        [Fact]
        public async Task CreateEventAsync_WithNegativeTeardownTime_ReturnsBadRequest()
        {
            // Arrange
            var eventModel = new EventModel
            {
                Id = 1,
                Name = "Test Event",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                SetupTime = 60,
                TeardownTime = -30 // Invalid: negative teardown time
            };

            // Act
            var result = await _controller.CreateEventAsync(eventModel);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var modelState = Assert.IsType<SerializableError>(badRequestResult.Value);
            Assert.True(modelState.ContainsKey("TeardownTime"));
        }

        [Fact]
        public async Task CreateEventAsync_WhenExceptionOccurs_ReturnsInternalServerError()
        {
            // Arrange
            var eventModel = new EventModel
            {
                Id = 1,
                Name = "Test Event",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                SetupTime = 60,
                TeardownTime = 30
            };

            _mapperMock.Setup(m => m.Map<EventDto>(It.IsAny<EventModel>()))
                .Returns(new EventDto());

            _eventServiceMock.Setup(s => s.CreateEventAsync(It.IsAny<EventDto>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.CreateEventAsync(eventModel);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        #endregion

        #region UpdateEvent Tests

        [Fact]
        public void UpdateEvent_WithValidModel_ReturnsOkResult()
        {
            // Arrange
            var eventModel = new EventModel
            {
                Id = 1,
                Name = "Test Event",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                SetupTime = 60,
                TeardownTime = 30
            };

            var eventDto = new EventDto
            {
                Id = 1,
                Name = "Test Event"
            };

            _mapperMock.Setup(m => m.Map<EventDto>(It.IsAny<EventModel>()))
                .Returns(eventDto);

            _eventServiceMock.Setup(s => s.UpdateEvent(It.IsAny<EventDto>()));

            // Act
            var result = _controller.UpdateEvent(eventModel);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void UpdateEvent_WithInvalidDates_ReturnsBadRequest()
        {
            // Arrange
            var eventModel = new EventModel
            {
                Id = 1,
                Name = "Test Event",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(-1), // Invalid: end date before start date
                SetupTime = 60,
                TeardownTime = 30
            };

            // Act
            var result = _controller.UpdateEvent(eventModel);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var modelState = Assert.IsType<SerializableError>(badRequestResult.Value);
            Assert.True(modelState.ContainsKey("EndDate"));
        }

        [Fact]
        public void UpdateEvent_WhenExceptionOccurs_ReturnsInternalServerError()
        {
            // Arrange
            var eventModel = new EventModel
            {
                Id = 1,
                Name = "Test Event",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                SetupTime = 60,
                TeardownTime = 30
            };

            _mapperMock.Setup(m => m.Map<EventDto>(It.IsAny<EventModel>()))
                .Returns(new EventDto());

            _eventServiceMock.Setup(s => s.UpdateEvent(It.IsAny<EventDto>()))
                .Throws(new Exception("Test exception"));

            // Act
            var result = _controller.UpdateEvent(eventModel);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        #endregion

        #region DeleteEvent Tests

        [Fact]
        public void DeleteEvent_WithValidId_ReturnsOkResult()
        {
            // Arrange
            _eventServiceMock.Setup(s => s.DeleteEvent(It.IsAny<int>()));

            // Act
            var result = _controller.DeleteEvent(1);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void DeleteEvent_WithInvalidId_ReturnsInternalServerError()
        {
            // Arrange - no setup needed, controller will throw exception for id <= 0

            // Act
            var result = _controller.DeleteEvent(0);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [Fact]
        public void DeleteEvent_WhenExceptionOccurs_ReturnsInternalServerError()
        {
            // Arrange
            _eventServiceMock.Setup(s => s.DeleteEvent(It.IsAny<int>()))
                .Throws(new Exception("Test exception"));

            // Act
            var result = _controller.DeleteEvent(1);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        #endregion
    }
}

