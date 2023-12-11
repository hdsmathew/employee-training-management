using Core.Application.Models;
using Core.Domain.Enrollment;

namespace Core.Application.Services
{
    public interface IEnrollmentService
    {
        Response<Enrollment> Process(Enrollment enrollment);
        Response<Enrollment> Submit(Enrollment enrollment);
        Response<Enrollment> ValidateApprovedEnrollments();
    }
}
