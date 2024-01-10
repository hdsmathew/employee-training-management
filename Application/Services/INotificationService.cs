using Core.Application.Models;
using Core.Domain;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    public interface INotificationService
    {
        Task<ResponseModel<EnrollmentNotification>> GetUnSeenEnrollmentNotificationsAsync(short recipientId);
        Task<ResponseModel<EnrollmentNotification>> SendEnrollmentNotificationAsync(string notificationMessage, short recipientId);
    }
}
