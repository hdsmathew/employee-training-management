using Infrastructure.Entities;
using System.Collections.Generic;

namespace Infrastructure.DAL
{
    public interface IEnrollmentDAL
    {
        int Add(EnrollmentEntity enrollment);
        int Delete(int enrollmentID);
        EnrollmentEntity Get(int enrollmentID);
        IEnumerable<EnrollmentEntity> GetAll();
        int Update(EnrollmentEntity enrollment);
    }
}
