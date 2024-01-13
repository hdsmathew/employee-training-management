using Core.Domain;
using Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.DAL.Interfaces
{
    public interface IUserNotificationDAL
    {
        Task<int> AddAsync(UserNotificationModel model);
        Task<int> AddBatchAsync(IEnumerable<UserNotificationModel> models);
        Task<IEnumerable<UserNotificationModel>> GetAllByRecipientIdAndSeenStatusAsync(short recipientId, bool hasSeen);
        Task<int> UpdateAsync(UserNotificationModel model);
    }
}
