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

        public DbSet<SeatStatus> SeatStatuses { get; set; }

        public DbSet<VenueSection> VenueSections { get; set; }

        public DbSet<VenueRow> VenueRows { get; set; }

        public DbSet<VenueSeat> VenueSeats { get; set; }

        public DbSet<EventSeat> EventSeats { get; set; }

        public DbSet<EventSeatStatus> EventSeatStatuses { get; set; }

        public DbSet<EventVenue> EventVenues { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderStatus> OrderStatuses { get; set; }

        public DbSet<CartStatus> CartStatuses { get; set; }

        public DbSet<PaymentStatus> PaymentStatuses { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Price> Prices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Configure composite keys

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

            modelBuilder.Entity<VenueSection>()
                .HasOne(vs => vs.Venue)
                .WithMany(v => v.VenueSections)
                .HasForeignKey(vs => vs.VenueId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            modelBuilder.Entity<VenueRow>()
                .HasOne(vr => vr.Section)
                .WithMany(vs => vs.VenueRows)
                .HasForeignKey(vr => vr.SectionId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            modelBuilder.Entity<VenueSeat>()
                .HasOne(vs => vs.Row)
                .WithMany(vr => vr.VenueSeats)
                .HasForeignKey(vs => vs.RowId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            modelBuilder.Entity<VenueSeat>()
                .HasOne(vs => vs.SeatsType)
                .WithMany(st => st.VenueSeats)
                .HasForeignKey(vs => vs.SeatsTypeId)
                .IsRequired();

            modelBuilder.Entity<EventSeat>()
                .HasOne(es => es.Status)
                .WithMany(ss => ss.EventSeats)
                .HasForeignKey(es => es.StatusId)
                .IsRequired();

            modelBuilder.Entity<Cart>()
                .Property(e => e.Id)
                .ValueGeneratedNever();


            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TimeStamp).IsRequired();

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Orders)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Status)
                    .WithMany(s => s.Orders)
                    .HasForeignKey(e => e.StatusId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TimeStamp).IsRequired();

                entity.HasOne(e => e.Status)
                    .WithMany(s => s.Payments)
                    .HasForeignKey(e => e.StatusId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Order)
                    .WithOne(o => o.Payment)
                    .HasForeignKey<Payment>(e => e.OrderId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Price>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasPrecision(18, 2);
            });



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
