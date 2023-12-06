using Core.Application.Repositories;
using Core.Domain.Enrollment;

namespace Core.Application.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;

        public EnrollmentService(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public void Process(Enrollment enrollment)
        {
            throw new System.NotImplementedException();
        }

        public void Submit(Enrollment enrollment)
        {
            throw new System.NotImplementedException();
        }

        public void ValidateEnrollments()
        {
            throw new System.NotImplementedException();
        }
    }
}
