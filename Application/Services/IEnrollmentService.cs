using Core.Application.Models;
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
        Task<Result> SubmitAsync(short employeeId, EnrollmentSubmissionViewModel enrollmentSubmissionViewModel);
        Task<ResultT<IEnumerable<(string, Result)>>> ValidateApprovedEnrollmentsAsync(short? approverAccountId);
        Task<Result> ValidateApprovedEnrollmentsByTrainingAsync(short approverAccountId, short trainingId);
    }
}
