using Core.Domain;
using Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.DAL.Interfaces
{
    public interface IEnrollmentNotificationDAL
    {
        Task<int> AddAsync(EnrollmentNotificationModel model);
        Task<int> AddBatchAsync(IEnumerable<EnrollmentNotificationModel> models);
        Task<IEnumerable<EnrollmentNotificationModel>> GetAllByRecipientIdAndSeenStatusAsync(short recipientId, bool hasSeen);
        Task<int> UpdateAsync(EnrollmentNotificationModel model);
    }
}
