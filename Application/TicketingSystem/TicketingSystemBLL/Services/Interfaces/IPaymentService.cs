using System;
using System.Threading.Tasks;
using TicketingSystemBLL.DTO;
using TicketingSystemBLL.Enums;

namespace TicketingSystemBLL.Services.Interfaces
{
    /// <summary>
    /// Main interface for the payment service.
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Method to retrieve payment by Id. 
        /// </summary>
        /// <param name="paymentId">Unique identifier for the payment.</param>
        /// <returns>Dto with the data related to payment.</returns>
        Task<PaymentDto> GetPaymentAsync(Guid paymentId);

        /// <summary>
        /// Method to retrieve payment status.
        /// </summary>
        /// <param name="paymentId">Unique identifier for the payment.</param>
        /// <param name="status">Status that will be assigned to the selected payment.</param>
        /// <returns>Task after code execution.</returns>
        Task<PaymentDto> UpdatePaymentStatusAsync(Guid paymentId, PaymentStatus status);
    }
}
