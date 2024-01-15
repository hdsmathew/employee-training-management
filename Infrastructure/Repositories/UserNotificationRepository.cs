using Core.Application.Repositories;
using Core.Domain;
using Infrastructure.Common;
using Infrastructure.DAL.Interfaces;
using Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserNotificationRepository : IUserNotificationRepository
    {
        private readonly IUserNotificationDAL _userNotificationDAL;
        private readonly MapperBase<UserNotification, UserNotificationModel> _userNotificationMapper;

        public UserNotificationRepository(IUserNotificationDAL userNotificationDAL, UserNotificationMapper userNotificationMapper)
        {
            _userNotificationDAL = userNotificationDAL;
            _userNotificationMapper = userNotificationMapper;
        }

        public Task Add(UserNotification userNotification)
        {
            UserNotificationModel userNotificationModel = _userNotificationMapper.MapEntityToDataModel(userNotification);
            return _userNotificationDAL.AddAsync(userNotificationModel);
        }

        public Task AddBatch(IEnumerable<UserNotification> userNotifications)
        {
            IEnumerable<UserNotificationModel> userNotificationModels = _userNotificationMapper.MapEntitiesToDataModels(userNotifications);
            return _userNotificationDAL.AddBatchAsync(userNotificationModels);
        }

        public async Task<IEnumerable<UserNotification>> GetAllByRecipientIdAndSeenStatusAsync(short recipientId, bool hasSeen)
        {
            IEnumerable<UserNotificationModel> userNotificationModels = await _userNotificationDAL.GetAllByRecipientIdAndSeenStatusAsync(recipientId, hasSeen);
            return _userNotificationMapper.MapDataModelsToEntities(userNotificationModels);
        }

        public Task Update(UserNotification userNotification)
        {
            UserNotificationModel userNotificationModel = _userNotificationMapper.MapEntityToDataModel(userNotification);
            return _userNotificationDAL.UpdateAsync(userNotificationModel);
        }
    }
}
