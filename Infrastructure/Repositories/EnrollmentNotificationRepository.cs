using Core.Application.Repositories;
using Core.Domain;
using Infrastructure.Common;
using Infrastructure.DAL.Interfaces;
using Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EnrollmentNotificationRepository : IEnrollmentNotificationRepository
    {
        private readonly IEnrollmentNotificationDAL _enrollmentNotificationDAL;
        private readonly MapperBase<EnrollmentNotification, EnrollmentNotificationModel> _enrollmentNotificationMapper;

        public EnrollmentNotificationRepository(IEnrollmentNotificationDAL enrollmentNotificationDAL, EnrollmentNotificationMapper enrollmentNotificationMapper)
        {
            _enrollmentNotificationDAL = enrollmentNotificationDAL;
            _enrollmentNotificationMapper = enrollmentNotificationMapper;
        }

        public Task<int> Add(EnrollmentNotification enrollmentNotification)
        {
            EnrollmentNotificationModel enrollmentNotificationModel = _enrollmentNotificationMapper.MapEntityToDataModel(enrollmentNotification);
            return _enrollmentNotificationDAL.AddAsync(enrollmentNotificationModel);
        }

        public Task<int> AddBatch(IEnumerable<EnrollmentNotification> enrollmentNotifications)
        {
            IEnumerable<EnrollmentNotificationModel> enrollmentNotificationModels = _enrollmentNotificationMapper.MapEntitiesToDataModels(enrollmentNotifications);
            return _enrollmentNotificationDAL.AddBatchAsync(enrollmentNotificationModels);
        }

        public async Task<IEnumerable<EnrollmentNotification>> GetAllByRecipientIdAndSeenStatusAsync(short recipientId, bool hasSeen)
        {
            IEnumerable<EnrollmentNotificationModel> enrollmentNotificationModels = await _enrollmentNotificationDAL.GetAllByRecipientIdAndSeenStatusAsync(recipientId, hasSeen);
            return _enrollmentNotificationMapper.MapDataModelsToEntities(enrollmentNotificationModels);
        }

        public Task<int> Update(EnrollmentNotification enrollmentNotification)
        {
            EnrollmentNotificationModel enrollmentNotificationModel = _enrollmentNotificationMapper.MapEntityToDataModel(enrollmentNotification);
            return _enrollmentNotificationDAL.UpdateAsync(enrollmentNotificationModel);
        }
    }
}
