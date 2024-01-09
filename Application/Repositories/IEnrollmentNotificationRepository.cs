using Core.Domain;
using System.Collections.Generic;

namespace Core.Application.Repositories
{
    public interface IEnrollmentNotificationRepository
    {
        int Add(EnrollmentNotification enrollmentNotification);
        int AddBatch(IEnumerable<EnrollmentNotification> enrollmentNotifications);
        IEnumerable<EnrollmentNotification> GetAllByRecipientIdAndSeenStatus(short recipientId, bool hasSeen);
        int Update(EnrollmentNotification enrollmentNotification);
    }
}
