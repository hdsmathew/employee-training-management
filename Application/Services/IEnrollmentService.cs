using Core.Domain.Enrollment;

namespace Core.Application.Services
{
    public interface IEnrollmentService
    {
        void Process(Enrollment enrollment);
        void Submit(Enrollment enrollment);
        void ValidateEnrollments();
    }
}
