using System;

namespace NotificationService.Models
{
    public class EmailResponse
    {
        public bool IsSuccess { get; set; }
        public string MessageId { get; set; }
        public string ErrorMessage { get; set; }
        public int StatusCode { get; set; }
        public Guid TrackingId { get; set; }
    }
}
