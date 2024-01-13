using System;

namespace Core.Domain
{
    public class UserNotification : IEntity
    {
        public UserNotification() { }

        public UserNotification(string title, string notificationMessage, short recipientId)
        {
            HasSeen = false;
            RecipientId = recipientId;
            NotificationMessage = notificationMessage;
            SentAt = DateTime.UtcNow;
            Title = title;
        }

        public int UserNotificationId { get; set; }
        public bool HasSeen { get; set; }
        public string NotificationMessage { get; set; }
        public short RecipientId { get; set; }
        public string Title { get; set; }
        public DateTime SeenAt { get; set; }
        public DateTime SentAt { get; set; }
    }
}
