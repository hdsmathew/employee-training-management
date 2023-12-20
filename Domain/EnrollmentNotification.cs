using System;

namespace Core.Domain
{
    public class EnrollmentNotification
    {
        public uint EnrollmentNotificationId { get; set; }
        public uint EnrollmentId { get; set; }
        public bool HasSeen { get; set; }
        public string NotificationMessage { get; set; }
        public ushort RecipientId { get; set; }
        public DateTime SeenAt { get; set; }
        public DateTime SentAt { get; set; }
    }
}
