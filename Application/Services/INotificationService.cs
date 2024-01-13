using Core.Application.Models;
using Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    public interface INotificationService
    {
        Task<ResultT<IEnumerable<EnrollmentNotification>>> GetUnSeenEnrollmentNotificationsAsync(short recipientId);
        Task<Result> SendEnrollmentNotificationAsync(string notificationMessage, short recipientId);
    }
}
