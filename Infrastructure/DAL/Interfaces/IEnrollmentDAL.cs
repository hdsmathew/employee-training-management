using Core.Domain;
using Infrastructure.Models;
using System.Collections.Generic;

namespace Infrastructure.DAL.Interfaces
{
    public interface IEnrollmentDAL
    {
        int Add(EnrollmentModel enrollment);
        int AddWithEmployeeUploads(EnrollmentModel enrollment, IEnumerable<EmployeeUploadModel> employeeUploads);
        int Delete(int enrollmentID);
        bool Exists(short employeeID, short trainingID);
        EnrollmentModel Get(int enrollmentID);
        IEnumerable<EnrollmentModel> GetAll();
        IEnumerable<EnrollmentModel> GetAllByTrainingIdAndApprovalStatus(short trainingId, IEnumerable<ApprovalStatusEnum> approvalStatusEnums);
        IEnumerable<EnrollmentModel> GetAllByEmployeeIdAndApprovalStatus(short employeeId, IEnumerable<ApprovalStatusEnum> approvalStatusEnums);
        IEnumerable<EnrollmentModel> GetAllByManagerIdAndApprovalStatus(short managerId, IEnumerable<ApprovalStatusEnum> approvalStatusEnums);
        int Update(EnrollmentModel enrollment);
        int UpdateBatch(IEnumerable<EnrollmentModel> enrollments);
    }
}
