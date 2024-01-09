using Core.Application.Repositories;
using Core.Domain;
using Infrastructure.Common;
using Infrastructure.DAL.Interfaces;
using Infrastructure.Models;
using System.Collections.Generic;

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

        public int Add(EnrollmentNotification enrollmentNotification)
        {
            EnrollmentNotificationModel enrollmentNotificationModel = _enrollmentNotificationMapper.MapEntityToDataModel(enrollmentNotification);
            return _enrollmentNotificationDAL.Add(enrollmentNotificationModel);
        }

        public int AddBatch(IEnumerable<EnrollmentNotification> enrollmentNotifications)
        {
            IEnumerable<EnrollmentNotificationModel> enrollmentNotificationModels = _enrollmentNotificationMapper.MapEntitiesToDataModels(enrollmentNotifications);
            return _enrollmentNotificationDAL.AddBatch(enrollmentNotificationModels);
        }

        public IEnumerable<EnrollmentNotification> GetAllByRecipientIdAndSeenStatus(short recipientId, bool hasSeen)
        {
            IEnumerable<EnrollmentNotificationModel> enrollmentNotificationModels = _enrollmentNotificationDAL.GetAllByRecipientIdAndSeenStatus(recipientId, hasSeen);
            return _enrollmentNotificationMapper.MapDataModelsToEntities(enrollmentNotificationModels);
        }

        public int Update(EnrollmentNotification enrollmentNotification)
        {
            EnrollmentNotificationModel enrollmentNotificationModel = _enrollmentNotificationMapper.MapEntityToDataModel(enrollmentNotification);
            return _enrollmentNotificationDAL.Update(enrollmentNotificationModel);
        }
    }
}
