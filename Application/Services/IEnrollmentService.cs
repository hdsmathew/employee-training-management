using Core.Application.Models;
using Core.Domain;

namespace Core.Application.Services
{
    public interface IEnrollmentService
    {
        ResponseModel<Enrollment> Process(Enrollment enrollment);
        ResponseModel<Enrollment> Submit(Enrollment enrollment);
        ResponseModel<Enrollment> ValidateApprovedEnrollments();
    }
}
