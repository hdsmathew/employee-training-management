using System;

namespace Infrastructure.Models
{
    public class UserNotificationModel : IModel
    {
        public int UserNotificationId { get; set; }
        public bool HasSeen { get; set; }
        public string NotificationMessage { get; set; }
        public short RecipientId { get; set; }
        public string Title { get; set; }
        public DateTime SeenAt { get; set; }
        public DateTime SentAt { get; set; }
    }
}
