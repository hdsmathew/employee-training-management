using Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Repositories
{
    public interface IEnrollmentRepository
    {
        Task Add(Enrollment enrollment);
        Task AddWithEmployeeUploads(Enrollment enrollment, IEnumerable<EmployeeUpload> employeeUploads);
        Task Delete(int enrollmentID);
        Task<bool> Exists(short employeeID, short trainingID);
        Task<Enrollment> GetAsync(int enrollmentID);
        Task<IEnumerable<Enrollment>> GetAllAsync();
        Task<IEnumerable<Enrollment>> GetAllByEmployeeIdAndApprovalStatusAsync(short employeeId, IEnumerable<ApprovalStatusEnum> approvalStatusEnums);
        Task<IEnumerable<Enrollment>> GetAllByManagerIdAndApprovalStatusAsync(short managerId, IEnumerable<ApprovalStatusEnum> approvalStatusEnums);
        Task<IEnumerable<Enrollment>> GetAllByTrainingIdAndApprovalStatusAsync(short trainingId, IEnumerable<ApprovalStatusEnum> approvalStatusEnums);
        Task Update(Enrollment enrollment);
        Task UpdateBatch(IEnumerable<Enrollment> enrollments);
    }
}
