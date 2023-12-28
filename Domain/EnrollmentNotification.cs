using System;

namespace Core.Domain
{
    public class EnrollmentNotification
    {
        public int EnrollmentNotificationId { get; set; }
        public int EnrollmentId { get; set; }
        public bool HasSeen { get; set; }
        public string NotificationMessage { get; set; }
        public short RecipientId { get; set; }
        public DateTime SeenAt { get; set; }
        public DateTime SentAt { get; set; }
    }
}
