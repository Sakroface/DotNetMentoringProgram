using Microsoft.EntityFrameworkCore;
using TicketingSystemDAL.Entities;

namespace TicketingSystemDAL.EntityFramework
{
    public class TicketingSystemDbContext : DbContext
    {
        public TicketingSystemDbContext(DbContextOptions<TicketingSystemDbContext> options)
            : base(options) { }

        public DbSet<Event> Events { get; set; }
        public DbSet<EventStatus> EventStatuses { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<VenueType> VenueTypes { get; set; }
        public DbSet<SeatsType> SeatsTypes { get; set; }
        public DbSet<VenueSection> VenueSections { get; set; }
        public DbSet<VenueRow> VenueRows { get; set; }
        public DbSet<VenueSeat> VenueSeats { get; set; }
        public DbSet<EventSeat> EventSeats { get; set; }
        public DbSet<EventSeatStatus> EventSeatStatuses { get; set; }
        public DbSet<EventVenue> EventVenues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Configure composite keys

            modelBuilder.Entity<EventSeat>()
                .HasKey(es => new { es.SeatId, es.EventId });

            modelBuilder.Entity<EventVenue>()
                .HasKey(ev => new { ev.EventId, ev.VenueId });

            #endregion

            #region Configure relationships

            modelBuilder.Entity<Event>()
                .HasOne(e => e.Status)
                .WithMany(s => s.Events)
                .HasForeignKey(e => e.StatusId)
                .IsRequired();

            modelBuilder.Entity<Venue>()
                .HasOne(v => v.VenueType)
                .WithMany(vt => vt.Venues)
                .HasForeignKey(v => v.VenueTypeId)
                .IsRequired();

            modelBuilder.Entity<VenueType>()
                .HasOne(vt => vt.SeatsType)
                .WithMany(st => st.VenueTypes)
                .HasForeignKey(vt => vt.SeatsTypeId)
                .IsRequired();

            modelBuilder.Entity<VenueSection>()
                .HasOne(vs => vs.Venue)
                .WithMany(v => v.VenueSections)
                .HasForeignKey(vs => vs.VenueId)
                .IsRequired();

            modelBuilder.Entity<VenueRow>()
                .HasOne(vr => vr.Section)
                .WithMany(vs => vs.VenueRows)
                .HasForeignKey(vr => vr.SectionId)
                .IsRequired();

            modelBuilder.Entity<VenueSeat>()
                .HasOne(vs => vs.Row)
                .WithMany(vr => vr.VenueSeats)
                .HasForeignKey(vs => vs.RowId)
                .IsRequired();

            modelBuilder.Entity<VenueSeat>()
                .HasOne(vs => vs.SeatsType)
                .WithMany(st => st.VenueSeats)
                .HasForeignKey(vs => vs.SeatsTypeId)
                .IsRequired();

            modelBuilder.Entity<EventSeat>()
                .HasOne(es => es.Seat)
                .WithMany(vs => vs.EventSeats)
                .HasForeignKey(es => es.SeatId)
                .IsRequired();

            modelBuilder.Entity<EventSeat>()
                .HasOne(es => es.Event)
                .WithMany(e => e.EventSeats)
                .HasForeignKey(es => es.EventId)
                .IsRequired();

            modelBuilder.Entity<EventSeat>()
                .HasOne(es => es.Status)
                .WithMany(ss => ss.EventSeats)
                .HasForeignKey(es => es.StatusId)
                .IsRequired();

            #region Many to many relationships configuration

            modelBuilder.Entity<EventVenue>()
                .HasOne(ev => ev.Event)
                .WithMany(e => e.EventVenues)
                .HasForeignKey(ev => ev.EventId)
                .IsRequired();

            modelBuilder.Entity<EventVenue>()
                .HasOne(ev => ev.Venue)
                .WithMany(v => v.EventVenues)
                .HasForeignKey(ev => ev.VenueId)
                .IsRequired();

            #endregion

            #endregion

            base.OnModelCreating(modelBuilder);
        }

    }
}
