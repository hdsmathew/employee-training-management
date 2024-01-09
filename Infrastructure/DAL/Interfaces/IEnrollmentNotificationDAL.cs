using Core.Domain;
using Infrastructure.Models;
using System.Collections.Generic;

namespace Infrastructure.DAL.Interfaces
{
    public interface IEnrollmentNotificationDAL
    {
        int Add(EnrollmentNotificationModel model);
        int AddBatch(IEnumerable<EnrollmentNotificationModel> models);
        IEnumerable<EnrollmentNotificationModel> GetAllByRecipientIdAndSeenStatus(short recipientId, bool hasSeen);
        int Update(EnrollmentNotificationModel model);
    }
}
