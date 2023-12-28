using Infrastructure.Entities;
using System.Collections.Generic;

namespace Infrastructure.DAL.Interfaces
{
    public interface IEnrollmentDAL
    {
        int Add(EnrollmentEntity enrollment);
        int Delete(int enrollmentID);
        bool Exists(short employeeID, short trainingID);
        EnrollmentEntity Get(int enrollmentID);
        IEnumerable<EnrollmentEntity> GetAll();
        int Update(EnrollmentEntity enrollment);
    }
}
