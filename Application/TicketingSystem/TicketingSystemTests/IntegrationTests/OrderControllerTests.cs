using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using TicketingSystem.Models;
using TicketingSystemDAL.Entities;
using TicketingSystemDAL.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace TicketingSystem.IntegrationTests
{
    public class OrderControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private Guid _testCartId;
        private int _testEventId;
        private int _testSeatId;
        private Guid _testPriceId;

        public class TestDbContext : TicketingSystemDbContext
        {
            public TestDbContext(DbContextOptions<TicketingSystemDbContext> options)
                : base(options) { }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
            }
        }

        public OrderControllerTests(WebApplicationFactory<Program> factory)
        {
            // Create a custom factory with mocked services if needed
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<TestDbContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    services.AddDbContext<TestDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDatabase");
                    });

                    var sp = services.BuildServiceProvider();
                    using var scope = sp.CreateScope();
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<TestDbContext>();

                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();
                    SeedTestData(db);
                });
            });

            _client = _factory.CreateClient();
        }



        [Fact]
        public async Task GetCart_WithValidId_ReturnsCart()
        {
            // Arrange
            var cartId = _testCartId.ToString();

            // Act
            var response = await _client.GetAsync($"/carts/{cartId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var cart = await response.Content.ReadFromJsonAsync<CartModel>();
            Assert.NotNull(cart);
            Assert.Equal(_testCartId, cart.Id);
        }

        [Fact]
        public async Task GetCart_WithInvalidId_ReturnsBadRequest()
        {
            // Arrange
            var invalidCartId = "not-a-guid";

            // Act
            var response = await _client.GetAsync($"/carts/{invalidCartId}");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task AddToCart_WithValidData_ReturnsUpdatedCart()
        {
            // Arrange
            var cartId = _testCartId.ToString();
            var cartItem = new CartItemModel
            {
                EventId = _testEventId,
                SeatId = _testSeatId,
                PriceId = _testPriceId.ToString()
            };

            // Act
            var response = await _client.PostAsJsonAsync($"/orders/carts/{cartId}", cartItem);

            // Assert
            response.EnsureSuccessStatusCode();
            var updatedCart = await response.Content.ReadFromJsonAsync<CartModel>();
            Assert.NotNull(updatedCart);
            Assert.Contains(updatedCart.EventSeats, seat => seat.SeatId == _testSeatId && seat.EventId == _testEventId);
        }

        [Fact]
        public async Task AddToCart_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var cartId = _testCartId.ToString();
            var invalidCartItem = new CartItemModel
            {
                // Missing required fields
                EventId = 0,
                SeatId = 0,
                PriceId = "not-a-guid"
            };

            // Act
            var response = await _client.PostAsJsonAsync($"/orders/carts/{cartId}", invalidCartItem);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RemoveSeatFromCart_WithValidData_ReturnsOk()
        {
            // Arrange
            var cartId = _testCartId.ToString();
            var eventId = _testEventId;
            var seatId = _testSeatId;

            // Act
            var response = await _client.DeleteAsync($"/orders/carts/{cartId}/events/{eventId}/seats/{seatId}");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task BookCart_WithValidId_ReturnsPaymentId()
        {
            // Arrange
            var cartId = _testCartId.ToString();

            // Act
            var response = await _client.PutAsync($"/orders/carts/{cartId}/book", null);

            // Assert
            response.EnsureSuccessStatusCode();
            var paymentId = await response.Content.ReadFromJsonAsync<Guid>();
            Assert.NotEqual(Guid.Empty, paymentId);
        }

        [Fact]
        public async Task BookCart_WithEmptyCart_ReturnsNotFound()
        {
            // Arrange
            var emptyCartId = Guid.NewGuid().ToString();

            // Act
            var response = await _client.PutAsync($"/orders/carts/{emptyCartId}/book", null);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        private void SeedTestData(TestDbContext dbContext)
        {
            var eventStatuses = InitEventStatuses(dbContext);
            var seatsTypes = InitSeatsTypes(dbContext);
            var venueTypes = InitVenueTypes(dbContext);
            var eventSeatStatuses = InitEventSeatStatuses(dbContext);
            var seatStatuses = InitSeatStatuses(dbContext);
            var orderStatuses = InitOrderStatuses(dbContext);
            var cartStatuses = InitCartStatuses(dbContext);
            var paymentStatuses = InitPaymentStatuses(dbContext);

            var eventStatus = eventStatuses.FirstOrDefault(x => x.Id == 1);
            var paymentStatus = paymentStatuses.FirstOrDefault(x => x.Id == 1);

            var testUser = new User { FirstName = "Test", UserName = "Test" };
            dbContext.Users.Add(testUser);
            dbContext.SaveChanges();

            var testEvent = new Event { Name = "Test Event", StatusId = eventStatus.Id, Status = eventStatus };
            dbContext.Events.Add(testEvent);
            dbContext.SaveChanges();
            _testEventId = testEvent.Id;

            var testVenue = new Venue { Name = "Test Venue", VenueTypeId = 1 };
            dbContext.Venues.Add(testVenue);
            dbContext.SaveChanges();

            var testVenueSection = new VenueSection { Name = "Test Venue Section", VenueId = testVenue.Id };
            dbContext.VenueSections.Add(testVenueSection);
            dbContext.SaveChanges();

            var testVenueRow = new VenueRow { Name = "Test Venue Row", SectionId = testVenueSection.Id };
            dbContext.VenueRows.Add(testVenueRow);
            dbContext.SaveChanges();

            var testVenueSeat = new VenueSeat { Number = 1, RowId = testVenueRow.Id, SeatsTypeId = 1 };
            dbContext.VenueSeats.Add(testVenueSeat);
            dbContext.SaveChanges();

            var testSeat = new EventSeat
            {
                EventId = testEvent.Id,
                StatusId = 1, 
                SeatId = testVenueSeat.Id
            };
            dbContext.EventSeats.Add(testSeat);
            dbContext.SaveChanges();
            _testSeatId = testSeat.Id;

            var testPrice = new Price
            {
                Amount = 50.00m,
                IsActive = true,
                SeatTypeId = testVenueSeat.SeatsTypeId
            };
            dbContext.Prices.Add(testPrice);
            dbContext.SaveChanges();
            _testPriceId = testPrice.Id;

            var testCart = new Cart
            {
                StatusId = 1,
                EventId = testEvent.Id,
                UserId = testUser.Id,
                EventSeats = new [] { testSeat }
            };
            dbContext.Carts.Add(testCart);
            dbContext.SaveChanges();
            _testCartId = testCart.Id;

            var testOrder = new Order
            {
                CartId = testCart.Id,
                UserId = testUser.Id,
                StatusId = 1
            };
            dbContext.Orders.Add(testOrder);
            dbContext.SaveChanges();

            var testPayment = new Payment
            {
                CartId = testCart.Id,
                OrderId = testOrder.Id,
                StatusId = paymentStatus.Id,
                Amount = testPrice.Amount,
                TimeStamp = DateTime.Now,
                Status = paymentStatus
            };
            dbContext.Payments.Add(testPayment);
            dbContext.SaveChanges();
        }

        private static IEnumerable<EventStatus> InitEventStatuses(TestDbContext context)
        {
            var eventStatusList = new List<EventStatus>()
            {
                new EventStatus() { Name = "Created" },
                new EventStatus() { Name = "Approved" },
                new EventStatus() { Name = "Setup" },
                new EventStatus() { Name = "InProgress" },
                new EventStatus() { Name = "Teardown" },
                new EventStatus() { Name = "Completed" },
                new EventStatus() { Name = "Cancelled" }
            };

            context.EventStatuses.AddRange(eventStatusList);
            context.SaveChanges();

            return context.EventStatuses;
        }

        private static IEnumerable<VenueType> InitVenueTypes(TestDbContext context)
        {
            var venueTypeList = new List<VenueType>()
            {
                new VenueType() { Name = "Stadium" },
                new VenueType() { Name = "FreeSpaceArena" },
            };

            context.VenueTypes.AddRange(venueTypeList);
            context.SaveChanges();

            return context.VenueTypes;
        }

        private static IEnumerable<EventSeatStatus> InitEventSeatStatuses(TestDbContext context)
        {
            var eventSeatStatusList = new List<EventSeatStatus>()
            {
                new EventSeatStatus() { Name = "Available" },
                new EventSeatStatus() { Name = "Booked" },
                new EventSeatStatus() { Name = "Assigned" }
            };

            context.EventSeatStatuses.AddRange(eventSeatStatusList);
            context.SaveChanges();

            return context.EventSeatStatuses;
        }

        private static IEnumerable<SeatsType> InitSeatsTypes(TestDbContext context)
        {
            var seatTypeList = new List<SeatsType>()
            {
                new SeatsType() { Name = "Basic" },
                new SeatsType() { Name = "VIP" }
            };

            context.SeatsTypes.AddRange(seatTypeList);
            context.SaveChanges();

            return context.SeatsTypes;
        }

        private static IEnumerable<SeatStatus> InitSeatStatuses(TestDbContext context)
        {
            var seatStatusList = new List<SeatStatus>()
            {
                new SeatStatus () { Name = "Available" },
                new SeatStatus () { Name = "NotAvailable" }
            };

            context.SeatStatuses.AddRange(seatStatusList);
            context.SaveChanges();

            return context.SeatStatuses;
        }

        private static IEnumerable<OrderStatus> InitOrderStatuses(TestDbContext context)
        {
            var seatStatusList = new List<OrderStatus>()
            {
                new OrderStatus () { Name = "Created" },
                new OrderStatus () { Name = "PaymentProcessed" },
                new OrderStatus () { Name = "Cancelled" },
            };

            context.OrderStatuses.AddRange(seatStatusList);
            context.SaveChanges();

            return context.OrderStatuses;
        }

        private static IEnumerable<CartStatus> InitCartStatuses(TestDbContext context)
        {
            var seatStatusList = new List<CartStatus>()
            {
                new CartStatus () { Name = "Created" },
                new CartStatus () { Name = "Processed" }
            };

            context.CartStatuses.AddRange(seatStatusList);
            context.SaveChanges();

            return context.CartStatuses;
        }

        private static IEnumerable<PaymentStatus> InitPaymentStatuses(TestDbContext context)
        {
            var seatStatusList = new List<PaymentStatus>()
            {
                new PaymentStatus () { Name = "Pending" },
                new PaymentStatus () { Name = "Completed" },
                new PaymentStatus () { Name = "Failed" }
            };

            context.PaymentStatuses.AddRange(seatStatusList);
            context.SaveChanges();

            return context.PaymentStatuses;
        }
    }
}
