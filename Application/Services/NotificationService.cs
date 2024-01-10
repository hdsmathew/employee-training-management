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

        public async Task<ResponseModel<EnrollmentNotification>> GetUnSeenEnrollmentNotificationsAsync(short recipientId)
        {
            ResponseModel<EnrollmentNotification> response = new ResponseModel<EnrollmentNotification>();
            try
            {
                IEnumerable<EnrollmentNotification> notifications = await _enrollmentNotificationRepository.GetAllByRecipientIdAndSeenStatusAsync(recipientId, false);
                response.Entities = notifications.OrderByDescending(notification => notification.SentAt);
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in retrieving enrollment notifications.");
                response.AddError(new ErrorModel()
                {
                    Message = $"Unable to retrieve enrollment notifications for recipient with id: {recipientId}.",
                    Exception = dalEx
                });
            }
            return response;
        }

        public async Task<ResponseModel<EnrollmentNotification>> SendEnrollmentNotificationAsync(string notificationMessage, short recipientId)
        {
            ResponseModel<EnrollmentNotification> response = new ResponseModel<EnrollmentNotification>();
            try
            {
                response.AddedRows = await _enrollmentNotificationRepository.Add(new EnrollmentNotification()
                {
                    HasSeen = false,
                    NotificationMessage = notificationMessage,
                    RecipientId = recipientId,
                    SentAt = DateTime.UtcNow
                });
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in sending enrollment notification.");
                response.AddError(new ErrorModel()
                {
                    Message = $"Unable to send enrollment notification to recipient with id: {recipientId}.",
                    Exception = dalEx
                });
            }
            return response;
        }
    }
}
