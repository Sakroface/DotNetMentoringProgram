using System;

namespace NotificationService.Models
{
    public class EmailRequest
    {
        public string To { get; set; }
        public string ToName { get; set; }
        public string From { get; set; }
        public string FromName { get; set; }
        public string Subject { get; set; }
        public string TextContent { get; set; }
        public string HtmlContent { get; set; }
        public Guid TrackingId { get; set; }
    }
}
