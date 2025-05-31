using System;
using System.Threading.Tasks;
using TicketingSystemDAL.Entities;

namespace TicketingSystemDAL.Repositories.Interfaces
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        public Task<Payment> GetPaymentByCartIdAsync(Guid id);
    }
}
