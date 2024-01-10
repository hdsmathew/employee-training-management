using Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Repositories
{
    public interface IEnrollmentNotificationRepository
    {
        Task<int> Add(EnrollmentNotification enrollmentNotification);
        Task<int> AddBatch(IEnumerable<EnrollmentNotification> enrollmentNotifications);
        Task<IEnumerable<EnrollmentNotification>> GetAllByRecipientIdAndSeenStatusAsync(short recipientId, bool hasSeen);
        Task<int> Update(EnrollmentNotification enrollmentNotification);
    }
}
