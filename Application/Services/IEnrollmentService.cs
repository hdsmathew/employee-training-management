using Core.Application.Models;
using Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    public interface IEnrollmentService
    {
        Task<Result> ApproveAsync(int enrollmentId, short approverAccountId);
        Task<Result> DeclineAsync(DeclineEnrollmentViewModel declineEnrollmentViewModel, short approverAccountId);
        Task<ResultT<IEnumerable<EnrollmentViewModel>>> GetEnrollmentsAsync(short employeeId);
        Task<ResultT<IEnumerable<EnrollmentViewModel>>> GetEnrollmentSubmissionsForApprovalAsync(short managerId);
        Task<Result> SubmitAsync(short employeeId, short trainingId, IEnumerable<EmployeeUpload> employeeUploads);
        Task<Result> SubmitAsync(short employeeId, short trainingId);
        Task<ResultT<IEnumerable<Result>>> ValidateApprovedEnrollmentsAsync(short? approverAccountId);
        Task<Result> ValidateApprovedEnrollmentsByTrainingAsync(short approverAccountId, short trainingId);
    }
}
