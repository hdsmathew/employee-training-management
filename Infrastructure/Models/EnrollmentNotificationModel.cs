using System;

namespace Infrastructure.Models
{
    public class EnrollmentNotificationModel : IModel
    {
        public int EnrollmentNotificationId { get; set; }
        public bool HasSeen { get; set; }
        public string NotificationMessage { get; set; }
        public short RecipientId { get; set; }
        public DateTime SeenAt { get; set; }
        public DateTime SentAt { get; set; }
    }
}
