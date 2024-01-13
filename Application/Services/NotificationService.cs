using Core.Application.Models;
using Core.Application.Repositories;
using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IEnrollmentNotificationRepository _enrollmentNotificationRepository;
        private readonly ILogger _logger;

        public NotificationService(IEnrollmentNotificationRepository enrollmentNotificationRepository, ILogger logger)
        {
            _enrollmentNotificationRepository = enrollmentNotificationRepository;
            _logger = logger;
        }

        public async Task<ResultT<IEnumerable<EnrollmentNotification>>> GetUnSeenEnrollmentNotificationsAsync(short recipientId)
        {
            try
            {
                IEnumerable<EnrollmentNotification> notifications = await _enrollmentNotificationRepository.GetAllByRecipientIdAndSeenStatusAsync(recipientId, false);

                return (notifications == null)
                    ? ResultT<IEnumerable<EnrollmentNotification>>.Failure(new Error("Could not retrieve notifications."))
                    : ResultT<IEnumerable<EnrollmentNotification>>.Success(notifications.OrderByDescending(notification => notification.SentAt));
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in retrieving enrollment notifications.");
                return ResultT<IEnumerable<EnrollmentNotification>>.Failure(new Error($"Unable to retrieve enrollment notifications for recipient with id: {recipientId}."));
            }
        }

        public async Task<Result> SendEnrollmentNotificationAsync(string notificationMessage, short recipientId)
        {
            try
            {
                await _enrollmentNotificationRepository.Add(new EnrollmentNotification()
                {
                    HasSeen = false,
                    NotificationMessage = notificationMessage,
                    RecipientId = recipientId,
                    SentAt = DateTime.UtcNow
                });

                return Result.Success();
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in sending enrollment notification.");
                return Result.Failure(new Error($"Unable to send enrollment notification to recipient with id: {recipientId}."));

            }
        }
    }
}
