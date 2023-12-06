using Core.Domain.Enrollment;
using System.Collections.Generic;

namespace Infrastructure.DAL
{
    public interface IEnrollmentDAL
    {
        bool Add(Enrollment enrollment);
        bool Delete(int enrollmentID);
        Enrollment Get(int enrollmentID);
        IEnumerable<Enrollment> GetAll();
        bool Update(Enrollment enrollment);
    }
}
