using Core.Domain;
using Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.DAL.Interfaces
{
    public interface IEnrollmentDAL
    {
        Task AddAsync(EnrollmentModel enrollment);
        Task AddWithEmployeeUploadsAsync(EnrollmentModel enrollment, IEnumerable<EmployeeUploadModel> employeeUploads);
        Task DeleteAsync(int enrollmentId);
        Task<bool> ExistsAsync(short employeeId, short trainingId);
        Task<EnrollmentModel> GetAsync(int enrollmentId);
        Task<IEnumerable<EnrollmentModel>> GetAllAsync();
        Task<IEnumerable<EnrollmentModel>> GetAllByTrainingIdAndApprovalStatusAsync(short trainingId, IEnumerable<ApprovalStatusEnum> approvalStatusEnums);
        Task<IEnumerable<EnrollmentModel>> GetAllByEmployeeIdAndApprovalStatusAsync(short employeeId, IEnumerable<ApprovalStatusEnum> approvalStatusEnums);
        Task<IEnumerable<EnrollmentModel>> GetAllByManagerIdAndApprovalStatusAsync(short managerId, IEnumerable<ApprovalStatusEnum> approvalStatusEnums);
        Task UpdateAsync(EnrollmentModel enrollment);
        Task UpdateBatchAsync(IEnumerable<EnrollmentModel> enrollments);
    }
}
