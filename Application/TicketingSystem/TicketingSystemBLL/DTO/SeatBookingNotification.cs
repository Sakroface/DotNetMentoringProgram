using System;
using System.Collections.Generic;

namespace TicketingSystemBLL.DTO
{
    public class SeatBookingNotification
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid NotificationTrackingId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Operation name
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Time stamp.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Notification parameters (customer email, customer name)
        /// </summary>
        public NotificationParameters Parameters { get; set; }

        /// <summary>
        /// Notification content (info that you need in the notification – order amount, order summary)
        /// </summary>
        public NotificationContent Content { get; set; }
    }

    public class NotificationParameters
    {
        public string CustomerEmail { get; set; }
        public string CustomerName { get; set; }
        public string UserId { get; set; }
    }

    public class NotificationContent
    {
        public decimal OrderAmount { get; set; }
        public string OrderSummary { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public string VenueName { get; set; }
    }

    public class OrderItem
    {
        public int SeatId { get; set; }
        public string SeatNumber { get; set; }
        public string SectionName { get; set; }
        public decimal Price { get; set; }
    }

}
