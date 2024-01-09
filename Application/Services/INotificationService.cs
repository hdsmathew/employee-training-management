using Core.Application.Models;
using Core.Domain;

namespace Core.Application.Services
{
    public interface INotificationService
    {
        ResponseModel<EnrollmentNotification> GetUnSeenEnrollmentNotifications(short recipientId);
        ResponseModel<EnrollmentNotification> SendEnrollmentNotification(string notificationMessage, short recipientId);
    }
}
