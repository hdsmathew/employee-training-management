using Core.Domain;
using System.Collections.Generic;

namespace Core.Application.Repositories
{
    public interface IEnrollmentRepository
    {
        int Add(Enrollment enrollment);
        int Delete(int enrollmentID);
        bool Exists(int employeeID, int trainingID);
        Enrollment Get(int enrollmentID);
        IEnumerable<Enrollment> GetAll();
        int Update(Enrollment enrollment);
    }
}
