using Core.Application.Repositories;
using Core.Domain.Enrollment;
using Infrastructure.DAL;
using System.Collections.Generic;

namespace Infrastructure.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly IEnrollmentDAL _enrollmentDAL;

        public EnrollmentRepository(IEnrollmentDAL enrollmentDAL)
        {
            _enrollmentDAL = enrollmentDAL;
        }

        public bool Add(Enrollment enrollment)
        {
            return _enrollmentDAL.Add(enrollment);
        }

        public bool Delete(int enrollmentID)
        {
            return _enrollmentDAL.Delete(enrollmentID);
        }

        public Enrollment Get(int enrollmentID)
        {
            return _enrollmentDAL.Get(enrollmentID);
        }

        public IEnumerable<Enrollment> GetAll()
        {
            return _enrollmentDAL.GetAll();
        }

        public bool Update(Enrollment enrollment)
        {
            return _enrollmentDAL.Update(enrollment);
        }
    }
}
