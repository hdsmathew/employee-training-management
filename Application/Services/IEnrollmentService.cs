using Core.Application.Models;
using Core.Domain;
using System.Collections.Generic;

namespace Core.Application.Services
{
    public interface IEnrollmentService
    {
        ResponseModel<EnrollmentViewModel> Approve(int enrollmentId, short approverAccountId);
        ResponseModel<EnrollmentViewModel> Decline(DeclineEnrollmentViewModel declineEnrollmentViewModel, short approverAccountId);
        ResponseModel<EnrollmentViewModel> GetEnrollments(short employeeId);
        ResponseModel<EnrollmentViewModel> GetEnrollmentSubmissionsForApproval(short managerId);
        ResponseModel<Enrollment> Submit(short employeeId, short trainingId, IEnumerable<EmployeeUpload> employeeUploads);
        ResponseModel<Enrollment> Submit(short employeeId, short trainingId);
        ResponseModel<ResponseModel<Enrollment>> ValidateApprovedEnrollments(short? approverAccountId);
        ResponseModel<Enrollment> ValidateApprovedEnrollmentsByTraining(short approverAccountId, short trainingId);
    }
}
