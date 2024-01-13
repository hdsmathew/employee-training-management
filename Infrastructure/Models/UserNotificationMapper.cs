using Core.Domain;
using Infrastructure.Common;
using System;

namespace Infrastructure.Models
{
    public class UserNotificationMapper : MapperBase<UserNotification, UserNotificationModel>
    {
        public override UserNotification MapDataModelToEntity(UserNotificationModel model)
        {
            if (model is null) return null;

            return new UserNotification
            {
                UserNotificationId = model.UserNotificationId,
                HasSeen = model.HasSeen,
                NotificationMessage = model.NotificationMessage,
                RecipientId = model.RecipientId,
                SeenAt = model.SeenAt,
                SentAt = model.SentAt,
                Title = model.Title
            };
        }

        public override UserNotificationModel MapEntityToDataModel(UserNotification entity)
        {
            if (entity is null) return null;

            return new UserNotificationModel
            {
                UserNotificationId = entity.UserNotificationId,
                HasSeen = entity.HasSeen,
                NotificationMessage = entity.NotificationMessage,
                RecipientId = entity.RecipientId,
                SeenAt = entity.SeenAt,
                SentAt = entity.SentAt,
                Title = entity.Title
            };
        }

        public override UserNotificationModel MapRowToDataModel((string, object)[] entityValueTuples)
        {
            if (entityValueTuples is null || entityValueTuples.Length == 0) return null;

            return new UserNotificationModel
            {
                UserNotificationId = GetValueFromTuple<int>("EnrollmentNotificationId", entityValueTuples),
                HasSeen = GetValueFromTuple<bool>("HasSeen", entityValueTuples),
                NotificationMessage = GetValueFromTuple<string>("NotificationMessage", entityValueTuples),
                RecipientId = GetValueFromTuple<short>("RecipientId", entityValueTuples),
                SeenAt = GetValueFromTuple<DateTime>("SeenAt", entityValueTuples),
                SentAt = GetValueFromTuple<DateTime>("SentAt", entityValueTuples),
                Title = GetValueFromTuple<string>("Title", entityValueTuples)
            };
        }
    }
}
