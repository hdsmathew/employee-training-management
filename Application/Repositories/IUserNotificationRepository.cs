using Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Repositories
{
    public interface IUserNotificationRepository
    {
        Task<int> Add(UserNotification userNotification);
        Task<int> AddBatch(IEnumerable<UserNotification> userNotifications);
        Task<IEnumerable<UserNotification>> GetAllByRecipientIdAndSeenStatusAsync(short recipientId, bool hasSeen);
        Task<int> Update(UserNotification userNotification);
    }
}
