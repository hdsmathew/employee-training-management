using Core.Application.Models;
using Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    public interface IEnrollmentService
    {
        Task<ResponseModel<EnrollmentViewModel>> ApproveAsync(int enrollmentId, short approverAccountId);
        Task<ResponseModel<EnrollmentViewModel>> DeclineAsync(DeclineEnrollmentViewModel declineEnrollmentViewModel, short approverAccountId);
        Task<ResponseModel<EnrollmentViewModel>> GetEnrollmentsAsync(short employeeId);
        Task<ResponseModel<EnrollmentViewModel> > GetEnrollmentSubmissionsForApprovalAsync(short managerId);
        Task<ResponseModel<Enrollment>> SubmitAsync(short employeeId, short trainingId, IEnumerable<EmployeeUpload> employeeUploads);
        Task<ResponseModel<Enrollment>> SubmitAsync(short employeeId, short trainingId);
        Task<ResponseModel<ResponseModel<Enrollment>>> ValidateApprovedEnrollmentsAsync(short? approverAccountId);
        Task<ResponseModel<Enrollment>> ValidateApprovedEnrollmentsByTrainingAsync(short approverAccountId, short trainingId);
    }
}
