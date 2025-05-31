using System;
using System.Threading.Tasks;
using TicketingSystemDAL.Entities;

namespace TicketingSystemDAL.Repositories.Interfaces
{
    public interface ICartRepository :  IRepository<Cart>
    {
        /// <summary>
        /// Method to get Cart with the seats.
        /// </summary>
        /// <param name="id">Id of the cart.</param>
        /// <returns>Cart with the related seats.</returns>
        public Task<Cart> GetCartWithSeatsAsync(Guid id);
    }
}
