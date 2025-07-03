using System;
using System.Data;
using System.Threading.Tasks;
using TicketingSystemDAL.Entities;
using TicketingSystemDAL.Repositories.Interfaces;

namespace TicketingSystemDAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<EventStatus> EventStatusRepository { get; }
        IRepository<VenueType> VenueTypeRepository { get; }
        IRepository<SeatsType> SeatsTypeRepository { get; }
        IRepository<VenueSection> VenueSectionRepository { get; }
        IRepository<VenueRow> VenueRowRepository { get; }
        IRepository<VenueSeat> VenueSeatRepository { get; }
        IRepository<EventSeat> EventSeatRepository { get; }
        IRepository<SeatStatus> SeatStatusRepository { get; }
        IRepository<EventVenue> EventVenueRepository { get; }
        IRepository<Price> PriceRepository { get; }
        IRepository<PaymentStatus> PaymentStatusRepository { get; }
        IRepository<OrderStatus> OrderStatusRepository { get; }
        IRepository<CartStatus> CartStatusRepository { get; }
        IPaymentRepository PaymentRepository { get; }
        IEventRepository EventRepository { get; }
        IVenueRepository VenueRepository { get; }
        IOrderRepository OrderRepository { get; }
        ICartRepository CartRepository { get; }
        IUserRepository UserRepository { get; }

        void Save();
        Task SaveAsync();
        void BeginTransaction();
        void BeginTransaction(IsolationLevel isolationLevel);
        Task BeginTransactionAsync();
        Task BeginTransactionAsync(IsolationLevel isolationLevel);
        void CommitTransaction();
        void RollbackTransaction();
    }
}
