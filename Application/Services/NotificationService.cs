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
        private readonly IUserNotificationRepository _userNotificationRepository;
        private readonly ILogger _logger;

        public NotificationService(IUserNotificationRepository userNotificationRepository, ILogger logger)
        {
            _userNotificationRepository = userNotificationRepository;
            _logger = logger;
        }

        public async Task<ResultT<IEnumerable<UserNotification>>> GetUnSeenEnrollmentNotificationsAsync(short recipientId)
        {
            try
            {
                IEnumerable<UserNotification> notifications = await _userNotificationRepository.GetAllByRecipientIdAndSeenStatusAsync(recipientId, false);

                return ResultT<IEnumerable<UserNotification>>.Success(notifications.OrderByDescending(notification => notification.SentAt));
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in retrieving enrollment notifications.");
                return ResultT<IEnumerable<UserNotification>>.Failure(new Error($"Unable to retrieve enrollment notifications for recipient with id: {recipientId}."));
            }
        }

        public async Task<Result> SendEnrollmentNotificationAsync(string notificationMessage, short recipientId)
        {
            try
            {
                await _userNotificationRepository.Add(new UserNotification()
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
