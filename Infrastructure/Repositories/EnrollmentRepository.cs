using Core.Application.Repositories;
using Core.Domain.Enrollment;
using Infrastructure.Common;
using Infrastructure.DAL;
using Infrastructure.Entities;
using System.Collections.Generic;

namespace Infrastructure.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly IEnrollmentDAL _enrollmentDAL;
        private readonly MapperBase<Enrollment, EnrollmentEntity> _enrollmentMapper;

        public EnrollmentRepository(IEnrollmentDAL enrollmentDAL, EnrollmentMapper enrollmentMapper)
        {
            _enrollmentDAL = enrollmentDAL;
            _enrollmentMapper = enrollmentMapper;
        }

        public int Add(Enrollment enrollment)
        {
            EnrollmentEntity enrollmentEntity = _enrollmentMapper.MapDomainModelToEntity(enrollment);
            return _enrollmentDAL.Add(enrollmentEntity);
        }

        public int Delete(int enrollmentID)
        {
            return _enrollmentDAL.Delete(enrollmentID);
        }

        public bool Exists(int employeeID, int trainingID)
        {
            return _enrollmentDAL.Exists(employeeID, trainingID);
        }

        public Enrollment Get(int enrollmentID)
        {
            EnrollmentEntity enrollmentEntity = _enrollmentDAL.Get(enrollmentID);
            return _enrollmentMapper.MapEntityToDomainModel(enrollmentEntity);
        }

        public IEnumerable<Enrollment> GetAll()
        {
            IEnumerable<EnrollmentEntity> enrollmentEntities = _enrollmentDAL.GetAll();
            return _enrollmentMapper.MapEntitiesToDomainModel(enrollmentEntities);
        }

        public int Update(Enrollment enrollment)
        {
            EnrollmentEntity enrollmentEntity = _enrollmentMapper.MapDomainModelToEntity(enrollment);
            return _enrollmentDAL.Update(enrollmentEntity);
        }
    }
}
