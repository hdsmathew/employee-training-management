using Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Repositories
{
    public interface IUserNotificationRepository
    {
        Task Add(UserNotification userNotification);
        Task AddBatch(IEnumerable<UserNotification> userNotifications);
        Task<IEnumerable<UserNotification>> GetAllByRecipientIdAndSeenStatusAsync(short recipientId, bool hasSeen);
        Task Update(UserNotification userNotification);
    }
}
