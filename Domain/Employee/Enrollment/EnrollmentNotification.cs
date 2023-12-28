using System;

namespace Core.Domain
{
    public class EnrollmentNotification
    {
        public int? EnrollmentNotificationId { get; set; } = null;
        public bool? HasSeen { get; set; } = null;
        public string NotificationMessage { get; set; } = null;
        public DateTime SeenAt { get; set; }
        public DateTime SentAt { get; set; }
    }
}
