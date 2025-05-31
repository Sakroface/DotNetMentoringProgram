using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystemDAL.Entities;
using TicketingSystemDAL.EntityFramework;
using TicketingSystemDAL.Repositories.Interfaces;

namespace TicketingSystemDAL.Repositories
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        public PaymentRepository(TicketingSystemDbContext context) : base(context)
        { }

        public async Task<Payment> GetPaymentByCartIdAsync(Guid id)
        {
            return await DbSet.Include(e => e.Cart)
                              .Include(e => e.Status)
                              .FirstOrDefaultAsync(e => e.CartId == id);
        }
    }
}
