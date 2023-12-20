using Core.Application.Repositories;
using Core.Domain;
using Infrastructure.Common;
using Infrastructure.DAL.Interfaces;
using Infrastructure.Entities;
using System.Collections.Generic;

namespace Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IEmployeeDAL _userDAL;
        private readonly MapperBase<Employee, EmployeeEntity> _userMapper;

        public EmployeeRepository(IEmployeeDAL userDAL, EmployeeMapper userMapper)
        {
            _userDAL = userDAL;
            _userMapper = userMapper;
        }

        public int Add(Employee user)
        {
            EmployeeEntity userEntity = _userMapper.MapDomainModelToEntity(user);
            return _userDAL.Add(userEntity);
        }

        public int Delete(int userID)
        {
            return _userDAL.Delete(userID);
        }

        public bool ExistsByNationalIdOrMobileNumber(string nationalId, string mobileNumber)
        {
            return _userDAL.ExistsByNationalIdOrMobileNumber(nationalId, mobileNumber);
        }

        public Employee Get(int userID)
        {
            EmployeeEntity userEntity = _userDAL.Get(userID);
            return _userMapper.MapEntityToDomainModel(userEntity);
        }

        public IEnumerable<Employee> GetAll()
        {
            IEnumerable<EmployeeEntity> userEntities = _userDAL.GetAll();
            return _userMapper.MapEntitiesToDomainModel(userEntities);
        }

        public int Update(Employee user)
        {
            EmployeeEntity userEntity = _userMapper.MapDomainModelToEntity(user);
            return _userDAL.Update(userEntity);
        }
    }
}
