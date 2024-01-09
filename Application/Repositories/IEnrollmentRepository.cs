using Core.Domain;
using System.Collections.Generic;

namespace Core.Application.Repositories
{
    public interface IEnrollmentRepository
    {
        int Add(Enrollment enrollment);
        int AddWithEmployeeUploads(Enrollment enrollment, IEnumerable<EmployeeUpload> employeeUploads);
        int Delete(int enrollmentID);
        bool Exists(short employeeID, short trainingID);
        Enrollment Get(int enrollmentID);
        IEnumerable<Enrollment> GetAll();
        IEnumerable<Enrollment> GetAllByEmployeeIdAndApprovalStatus(short employeeId, IEnumerable<ApprovalStatusEnum> approvalStatusEnums);
        IEnumerable<Enrollment> GetAllByManagerIdAndApprovalStatus(short managerId, IEnumerable<ApprovalStatusEnum> approvalStatusEnums);
        IEnumerable<Enrollment> GetAllByTrainingIdAndApprovalStatus(short trainingId, IEnumerable<ApprovalStatusEnum> approvalStatusEnums);
        int Update(Enrollment enrollment);
        int UpdateBatch(IEnumerable<Enrollment> enrollments);
    }
}
