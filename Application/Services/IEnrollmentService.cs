using Core.Application.Models;
using Core.Domain;

namespace Core.Application.Services
{
    public interface IEnrollmentService
    {
        ResponseModel<Enrollment> Process(Enrollment enrollment);
        ResponseModel<Enrollment> Submit(short employeeId, short trainingId);
        ResponseModel<Enrollment> ValidateApprovedEnrollments();
    }
}
