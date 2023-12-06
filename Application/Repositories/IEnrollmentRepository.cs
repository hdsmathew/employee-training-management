using Core.Domain.Enrollment;
using System.Collections.Generic;

namespace Core.Application.Repositories
{
    public interface IEnrollmentRepository
    {
        bool Add(Enrollment enrollment);
        bool Delete(int enrollmentID);
        Enrollment Get(int enrollmentID);
        IEnumerable<Enrollment> GetAll();
        bool Update(Enrollment enrollment);
    }
}
