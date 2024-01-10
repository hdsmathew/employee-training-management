using Core.Domain;
using Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.DAL.Interfaces
{
    public interface IEnrollmentDAL
    {
        Task<int> AddAsync(EnrollmentModel enrollment);
        Task<int> AddWithEmployeeUploadsAsync(EnrollmentModel enrollment, IEnumerable<EmployeeUploadModel> employeeUploads);
        Task<int> DeleteAsync(int enrollmentID);
        Task<bool> ExistsAsync(short employeeID, short trainingID);
        Task<EnrollmentModel> GetAsync(int enrollmentID);
        Task<IEnumerable<EnrollmentModel>> GetAllAsync();
        Task<IEnumerable<EnrollmentModel>> GetAllByTrainingIdAndApprovalStatusAsync(short trainingId, IEnumerable<ApprovalStatusEnum> approvalStatusEnums);
        Task<IEnumerable<EnrollmentModel>> GetAllByEmployeeIdAndApprovalStatusAsync(short employeeId, IEnumerable<ApprovalStatusEnum> approvalStatusEnums);
        Task<IEnumerable<EnrollmentModel>> GetAllByManagerIdAndApprovalStatusAsync(short managerId, IEnumerable<ApprovalStatusEnum> approvalStatusEnums);
        Task<int> UpdateAsync(EnrollmentModel enrollment);
        Task<int> UpdateBatchAsync(IEnumerable<EnrollmentModel> enrollments);
    }
}
