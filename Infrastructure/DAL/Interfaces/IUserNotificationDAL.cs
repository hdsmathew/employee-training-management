using Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.DAL.Interfaces
{
    public interface IUserNotificationDAL
    {
        Task AddAsync(UserNotificationModel model);
        Task AddBatchAsync(IEnumerable<UserNotificationModel> models);
        Task<IEnumerable<UserNotificationModel>> GetAllByRecipientIdAndSeenStatusAsync(short recipientId, bool hasSeen);
        Task UpdateAsync(UserNotificationModel model);
    }
}
