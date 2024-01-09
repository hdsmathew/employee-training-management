using Core.Application.Models;
using Core.Application.Repositories;
using Core.Domain;
using System;
using System.Linq;

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

        public ResponseModel<EnrollmentNotification> GetUnSeenEnrollmentNotifications(short recipientId)
        {
            ResponseModel<EnrollmentNotification> response = new ResponseModel<EnrollmentNotification>();
            try
            {
                response.Entities = _enrollmentNotificationRepository.GetAllByRecipientIdAndSeenStatus(recipientId, false)
                    .OrderByDescending(notification => notification.SentAt);
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

        public ResponseModel<EnrollmentNotification> SendEnrollmentNotification(string notificationMessage, short recipientId)
        {
            ResponseModel<EnrollmentNotification> response = new ResponseModel<EnrollmentNotification>();
            try
            {
                response.AddedRows = _enrollmentNotificationRepository.Add(new EnrollmentNotification()
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
