using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;
using TicketingSystemDAL.Entities;
using TicketingSystemDAL.EntityFramework;
using TicketingSystemDAL.Repositories;
using TicketingSystemDAL.Repositories.Interfaces;

namespace TicketingSystemDAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TicketingSystemDbContext _context;
        private IDbContextTransaction _transaction;
        private bool _disposed = false;

        private IRepository<EventStatus> _eventStatusRepository;
        private IRepository<VenueType> _venueTypeRepository;
        private IRepository<SeatsType> _seatsTypeRepository;
        private IRepository<VenueSection> _venueSectionRepository;
        private IRepository<VenueRow> _venueRowRepository;
        private IRepository<VenueSeat> _venueSeatRepository;
        private IRepository<EventSeat> _eventSeatRepository;
        private IRepository<SeatStatus> _seatStatusRepository;
        private IRepository<EventVenue> _eventVenueRepository;
        private IRepository<PaymentStatus> _paymentStatusRepository;
        private IRepository<OrderStatus> _orderStatusRepository;
        private IRepository<CartStatus> _cartStatusRepository;
        private IRepository<Price> _priceRepository;
        private IPaymentRepository _paymentRepository;
        private IEventRepository _eventRepository;
        private IVenueRepository _venueRepository;
        private IOrderRepository _orderRepository;
        private ICartRepository _cartRepository;
        private IUserRepository _userRepository;

        public UnitOfWork(TicketingSystemDbContext context)
        {
            _context = context;
        }

        public IRepository<EventStatus> EventStatusRepository =>
            _eventStatusRepository ??= new Repository<EventStatus>(_context);

        public IRepository<VenueType> VenueTypeRepository =>
            _venueTypeRepository ??= new Repository<VenueType>(_context);

        public IRepository<SeatsType> SeatsTypeRepository =>
            _seatsTypeRepository ??= new Repository<SeatsType>(_context);

        public IRepository<VenueSection> VenueSectionRepository =>
            _venueSectionRepository ??= new Repository<VenueSection>(_context);

        public IRepository<VenueRow> VenueRowRepository =>
            _venueRowRepository ??= new Repository<VenueRow>(_context);

        public IRepository<VenueSeat> VenueSeatRepository =>
            _venueSeatRepository ??= new Repository<VenueSeat>(_context);

        public IRepository<EventSeat> EventSeatRepository =>
            _eventSeatRepository ??= new Repository<EventSeat>(_context);

        public IRepository<SeatStatus> SeatStatusRepository =>
            _seatStatusRepository ??= new Repository<SeatStatus>(_context);

        public IRepository<EventVenue> EventVenueRepository =>
            _eventVenueRepository ??= new Repository<EventVenue>(_context);

        public IRepository<PaymentStatus> PaymentStatusRepository =>
            _paymentStatusRepository ??= new Repository<PaymentStatus>(_context);

        public IRepository<OrderStatus> OrderStatusRepository =>
            _orderStatusRepository ??= new Repository<OrderStatus>(_context);

        public IRepository<CartStatus> CartStatusRepository =>
            _cartStatusRepository ??= new Repository<CartStatus>(_context);

        public IRepository<Price> PriceRepository =>
            _priceRepository ??= new Repository<Price>(_context);

        public IPaymentRepository PaymentRepository =>
            _paymentRepository ??= new PaymentRepository(_context);

        public IEventRepository EventRepository =>
            _eventRepository ??= new EventRepository(_context);

        public IVenueRepository VenueRepository =>
            _venueRepository ??= new VenueRepository(_context);

        public ICartRepository CartRepository =>
           _cartRepository ??= new CartRepository(_context);

        public IOrderRepository OrderRepository =>
           _orderRepository ??= new OrderRepository(_context);

        public IUserRepository UserRepository =>
            _userRepository ??= new UserRepository(_context);

        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            try
            {
                _context.SaveChanges();
                _transaction?.Commit();
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _transaction?.Rollback();
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

}
