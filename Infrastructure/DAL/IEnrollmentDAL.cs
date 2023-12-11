using Infrastructure.Entities;
using System.Collections.Generic;

namespace Infrastructure.DAL
{
    public interface IEnrollmentDAL
    {
        int Add(EnrollmentEntity enrollment);
        int Delete(int enrollmentID);
        bool Exists(int employeeID, int trainingID);
        EnrollmentEntity Get(int enrollmentID);
        IEnumerable<EnrollmentEntity> GetAll();
        int Update(EnrollmentEntity enrollment);
    }
}
