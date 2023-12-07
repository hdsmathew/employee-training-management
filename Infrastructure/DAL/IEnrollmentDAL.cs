using Core.Domain.Enrollment;
using System.Collections.Generic;

namespace Infrastructure.DAL
{
    public interface IEnrollmentDAL
    {
        int Add(Enrollment enrollment);
        int Delete(int enrollmentID);
        Enrollment Get(int enrollmentID);
        IEnumerable<Enrollment> GetAll();
        int Update(Enrollment enrollment);
    }
}
