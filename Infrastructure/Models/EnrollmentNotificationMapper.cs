using Core.Domain;
using Infrastructure.Common;
using System;
using System.Reflection;

namespace Infrastructure.Models
{
    public class EnrollmentNotificationMapper : MapperBase<EnrollmentNotification, EnrollmentNotificationModel>
    {
        public override EnrollmentNotification MapDataModelToEntity(EnrollmentNotificationModel model)
        {
            if (model is null) return null;

            return new EnrollmentNotification
            {
                EnrollmentNotificationId = model.EnrollmentNotificationId,
                HasSeen = model.HasSeen,
                NotificationMessage = model.NotificationMessage,
                RecipientId = model.RecipientId,
                SeenAt = model.SeenAt,
                SentAt = model.SentAt
            };
        }

        public override EnrollmentNotificationModel MapEntityToDataModel(EnrollmentNotification entity)
        {
            if (entity is null) return null;

            return new EnrollmentNotificationModel
            {
                EnrollmentNotificationId = entity.EnrollmentNotificationId,
                HasSeen = entity.HasSeen,
                NotificationMessage = entity.NotificationMessage,
                RecipientId = entity.RecipientId,
                SeenAt = entity.SeenAt,
                SentAt = entity.SentAt
            };
        }

        public override EnrollmentNotificationModel MapRowToDataModel((string, object)[] entityValueTuples)
        {
            if (entityValueTuples is null || entityValueTuples.Length == 0) return null;

            return new EnrollmentNotificationModel
            {
                EnrollmentNotificationId = GetValueFromTuple<int>("EnrollmentNotificationId", entityValueTuples),
                HasSeen = GetValueFromTuple<bool>("HasSeen", entityValueTuples),
                NotificationMessage = GetValueFromTuple<string>("NotificationMessage", entityValueTuples),
                RecipientId = GetValueFromTuple<short>("RecipientId", entityValueTuples),
                SeenAt = GetValueFromTuple<DateTime>("SeenAt", entityValueTuples),
                SentAt = GetValueFromTuple<DateTime>("SentAt", entityValueTuples)
            };
        }
    }
}
