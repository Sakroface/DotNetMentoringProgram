using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TicketingSystem.MappingProfiles;
using TicketingSystemBLL.Interfaces;
using TicketingSystemBLL.Mapper;
using TicketingSystemBLL.Services;
using TicketingSystemBLL.Services.Interfaces;
using TicketingSystemDAL.Entities;
using TicketingSystemDAL.EntityFramework;
using TicketingSystemDAL.Repositories;
using TicketingSystemDAL.Repositories.Interfaces;
using TicketingSystemDAL.UnitOfWork;

namespace TicketingSystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TicketingSystem", Version = "v1" });
            });

            services.AddDbContext<TicketingSystemDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            services.AddScoped<IVenueRepository, VenueRepository>();
            services.AddScoped<IEventRepository, EventRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddAutoMapper(typeof(ModelsMapperProfile), typeof(BLLMapperProfile));
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IVenueService, VenueService>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TicketingSystemDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TicketingSystem v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            dbContext.Database.EnsureCreated();
            SeedData(dbContext);
        }

        private static void SeedData(TicketingSystemDbContext dbContext)
        {
            InitializeDictionaries(dbContext);
            InitializeEvents(dbContext);
            InitializeVenues(dbContext);

            dbContext.SaveChanges();
        }

        private static void InitializeDictionaries(TicketingSystemDbContext context)
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

            var venueTypeList = new List<VenueType>()
            {
                new VenueType() { Name = "Stadium" },
                new VenueType() { Name = "FreeSpaceArena" },
            };

            var eventSeatStatusList = new List<EventSeatStatus>()
            {
                new EventSeatStatus() { Name = "Available" },
                new EventSeatStatus() { Name = "Booked" },
                new EventSeatStatus() { Name = "Assigned" }
            };

            var seatTypeList = new List<SeatsType>()
            {
                new SeatsType() { Name = "Basic" },
                new SeatsType() { Name = "VIP" }
            };

            var seatStatusList = new List<SeatStatus>()
            {
                new SeatStatus () { Name = "Available" },
                new SeatStatus () { Name = "NotAvailable" }
            };

            context.EventStatuses.AddRange(eventStatusList);
            context.VenueTypes.AddRange(venueTypeList);
            context.EventSeatStatuses.AddRange(eventSeatStatusList);
            context.SeatsTypes.AddRange(seatTypeList);

            context.SaveChanges();
        }

        private static void InitializeEvents(TicketingSystemDbContext context)
        {
            var eventList = new List<Event>() {
                new Event()
                {
                    Name = "Cold Play Grand Concert.",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In lectus magna, lacinia eu semper non, rutrum ut nisi. Morbi volutpat gravida leo a efficitur. Vestibulum dapibus quam ex, ac semper sapien molestie et. Nunc ac nibh vitae arcu imperdiet tempus eget quis erat. Cras non lectus vitae odio blandit aliquam a id lectus. Nunc lacus dolor, pharetra vitae congue ac, mattis eget eros. Vivamus ultricies rhoncus elit. Sed et risus vitae turpis tincidunt lobortis vitae nec nisi. Duis tempus posuere tempus. Aenean quam mi, faucibus sit amet risus ut, pulvinar dignissim metus. Vestibulum turpis dui, dapibus ut nulla eget, fringilla iaculis dui.\n\t Duis ornare lectus at erat convallis blandit. Nunc id interdum nibh, in eleifend ex. Donec ac erat sed risus viverra pulvinar. Donec ac neque aliquet, blandit urna at, lobortis dolor. Nullam sit amet tortor neque. Vestibulum faucibus varius dui, eu cursus est finibus at. Vivamus nec semper sem. Mauris at aliquet enim. Nulla imperdiet risus quis tincidunt fermentum. Aenean eleifend vel metus at sollicitudin. Suspendisse luctus fermentum risus vitae porta.\n\tNam sed lectus id turpis suscipit elementum. Proin in justo ac nisi tincidunt dictum id ac metus. Nam at sagittis sapien. Etiam lacinia neque vitae est sodales congue. Sed lobortis, dolor sed vestibulum ultricies, libero leo euismod risus, sed pulvinar urna lectus sed velit. Aenean finibus vel justo id pharetra. Phasellus fermentum turpis nec mauris pretium bibendum. Vestibulum porttitor sapien risus, finibus porta nibh porta non. Donec a maximus erat. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Praesent iaculis risus non pretium vulputate. Etiam imperdiet posuere orci, non dignissim purus ullamcorper in. Curabitur tristique a velit a lobortis. Vestibulum fringilla dolor pulvinar, egestas lectus a, pellentesque arcu. Duis varius rhoncus metus, ut malesuada risus hendrerit et. Praesent quam ligula, mattis vitae elementum non, hendrerit eu sem.\n\tDonec elementum ligula erat, eget cursus felis lacinia quis. Aenean accumsan eget ex ac consectetur. Suspendisse fringilla tellus quis sem dictum sodales. Quisque a congue urna, non congue lorem. Cras volutpat mauris vitae sagittis porta. Cras metus mi, porta nec finibus vel, tempor eget lectus. Phasellus ut velit ornare, porta dui id, maximus magna. Etiam elit metus, tristique sit amet volutpat ut, feugiat sit amet erat. Aenean tellus nibh, vulputate eu sem non, convallis volutpat sem. Cras tortor lacus, viverra nec placerat ut, gravida sit amet nibh. Pellentesque sit amet consectetur lorem.\n\tSed faucibus sit amet magna id euismod. Vivamus volutpat, libero eu molestie efficitur, est tortor hendrerit neque, nec facilisis massa sapien id diam. Vestibulum euismod, tortor nec porta tempor, dolor nisi auctor magna, sit amet tristique eros sapien eu enim. In volutpat nulla sit amet justo ornare, sit amet congue magna fermentum. Integer eget pharetra dolor. Quisque consectetur pharetra nisl vel mollis. In accumsan leo sed dolor aliquet, eu pulvinar purus eleifend. Vivamus nulla purus, varius ac ipsum in, blandit molestie lorem. Aenean imperdiet ornare metus in tempus. Ut eu lacus pellentesque, dapibus tellus ac, tincidunt dolor. Aenean luctus posuere arcu, et molestie quam euismod id." ,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(2),
                    SetupTime = 2400,
                    TeardownTime = 2400,
                    StatusId = 4
                },
                new Event()
                {
                    Name = "President election debates",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In lectus magna, lacinia eu semper non, rutrum ut nisi. Morbi volutpat gravida leo a efficitur. Vestibulum dapibus quam ex, ac semper sapien molestie et. Nunc ac nibh vitae arcu imperdiet tempus eget quis erat. Cras non lectus vitae odio blandit aliquam a id lectus. Nunc lacus dolor, pharetra vitae congue ac, mattis eget eros. Vivamus ultricies rhoncus elit. Sed et risus vitae turpis tincidunt lobortis vitae nec nisi. Duis tempus posuere tempus. Aenean quam mi, faucibus sit amet risus ut, pulvinar dignissim metus. Vestibulum turpis dui, dapibus ut nulla eget, fringilla iaculis dui.\n\t Duis ornare lectus at erat convallis blandit. Nunc id interdum nibh, in eleifend ex. Donec ac erat sed risus viverra pulvinar. Donec ac neque aliquet, blandit urna at, lobortis dolor. Nullam sit amet tortor neque. Vestibulum faucibus varius dui, eu cursus est finibus at. Vivamus nec semper sem. Mauris at aliquet enim. Nulla imperdiet risus quis tincidunt fermentum. Aenean eleifend vel metus at sollicitudin. Suspendisse luctus fermentum risus vitae porta.\n\tNam sed lectus id turpis suscipit elementum. Proin in justo ac nisi tincidunt dictum id ac metus. Nam at sagittis sapien. Etiam lacinia neque vitae est sodales congue. Sed lobortis, dolor sed vestibulum ultricies, libero leo euismod risus, sed pulvinar urna lectus sed velit. Aenean finibus vel justo id pharetra. Phasellus fermentum turpis nec mauris pretium bibendum. Vestibulum porttitor sapien risus, finibus porta nibh porta non. Donec a maximus erat. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Praesent iaculis risus non pretium vulputate. Etiam imperdiet posuere orci, non dignissim purus ullamcorper in. Curabitur tristique a velit a lobortis. Vestibulum fringilla dolor pulvinar, egestas lectus a, pellentesque arcu. Duis varius rhoncus metus, ut malesuada risus hendrerit et. Praesent quam ligula, mattis vitae elementum non, hendrerit eu sem.\n\tDonec elementum ligula erat, eget cursus felis lacinia quis. Aenean accumsan eget ex ac consectetur. Suspendisse fringilla tellus quis sem dictum sodales. Quisque a congue urna, non congue lorem. Cras volutpat mauris vitae sagittis porta. Cras metus mi, porta nec finibus vel, tempor eget lectus. Phasellus ut velit ornare, porta dui id, maximus magna. Etiam elit metus, tristique sit amet volutpat ut, feugiat sit amet erat. Aenean tellus nibh, vulputate eu sem non, convallis volutpat sem. Cras tortor lacus, viverra nec placerat ut, gravida sit amet nibh. Pellentesque sit amet consectetur lorem.\n\tSed faucibus sit amet magna id euismod. Vivamus volutpat, libero eu molestie efficitur, est tortor hendrerit neque, nec facilisis massa sapien id diam. Vestibulum euismod, tortor nec porta tempor, dolor nisi auctor magna, sit amet tristique eros sapien eu enim. In volutpat nulla sit amet justo ornare, sit amet congue magna fermentum. Integer eget pharetra dolor. Quisque consectetur pharetra nisl vel mollis. In accumsan leo sed dolor aliquet, eu pulvinar purus eleifend. Vivamus nulla purus, varius ac ipsum in, blandit molestie lorem. Aenean imperdiet ornare metus in tempus. Ut eu lacus pellentesque, dapibus tellus ac, tincidunt dolor. Aenean luctus posuere arcu, et molestie quam euismod id." ,
                    StartDate = DateTime.Now.AddYears(1),
                    EndDate = DateTime.Now.AddYears(1).AddHours(2),
                    SetupTime = 4800,
                    TeardownTime = 4800,
                    StatusId = 1
                }
            };

            context.Events.AddRange(eventList);
        }

        private static void InitializeVenues(TicketingSystemDbContext context)
        {
            var venueList = new List<Venue>()
            {
                new Venue()
                {
                    Name = "Olympic stadium",
                    VenueTypeId = 1,
                    NumberOfSeats = 0,
                },
                new Venue()
                {
                    Name = "Dance arena",
                    VenueTypeId = 2,
                    NumberOfSeats = 1200,
                }
            };

            context.Venues.AddRange(venueList);
            context.SaveChanges();

            var olympicStadium = context.Venues.FirstOrDefault(venue => venue.VenueTypeId == 1);
            InitializeVenuesWithSeats(olympicStadium, context);
        }

        private static void InitializeVenuesWithSeats(Venue venue, TicketingSystemDbContext context)
        {
            Random random = new Random();
            int totalSeats = 0;

            for (int sectionNum = 0; sectionNum < 24; sectionNum++)
            {
                var section = new VenueSection
                {
                    VenueId = venue.Id,
                    Name = $"Section {sectionNum}"
                };

                context.VenueSections.Add(section);
                context.SaveChanges(); // Save to get the section ID

                for (int rowNum = 0; rowNum < 50; rowNum++)
                {
                    var row = new VenueRow
                    {
                        SectionId = section.Id,
                        Name = $"Row {rowNum}"
                    };

                    context.VenueRows.Add(row);
                    context.SaveChanges(); // Save to get the row ID

                    // Generate between 30-60 seats per row
                    var seatsInRow = random.Next(40, 61);

                    for (var seatNum = 0; seatNum <= seatsInRow; seatNum++)
                    {
                        var seat = new VenueSeat
                        {
                            RowId = row.Id,
                            Number = seatNum,
                            SeatsTypeId = 1
                        };

                        context.VenueSeats.Add(seat);
                        totalSeats++;
                    }
                }
            }

            venue.NumberOfSeats = totalSeats;
            context.SaveChanges();
        }


    }
}
