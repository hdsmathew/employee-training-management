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
        private readonly IEmployeeDAL _employeeDAL;
        private readonly MapperBase<Employee, EmployeeEntity> _employeeMapper;

        public EmployeeRepository(IEmployeeDAL employeeDAL, EmployeeMapper employeeMapper)
        {
            _employeeDAL = employeeDAL;
            _employeeMapper = employeeMapper;
        }

        public int Add(Employee employee)
        {
            EmployeeEntity employeeEntity = _employeeMapper.MapDomainModelToEntity(employee);
            return _employeeDAL.Add(employeeEntity);
        }

        public int Delete(int employeeId)
        {
            return _employeeDAL.Delete(employeeId);
        }

        public bool ExistsByNationalIdOrMobileNumber(string nationalId, string mobileNumber)
        {
            return _employeeDAL.ExistsByNationalIdOrMobileNumber(nationalId, mobileNumber);
        }

        public Employee Get(int employeeId)
        {
            EmployeeEntity employeeEntity = _employeeDAL.Get(employeeId);
            return _employeeMapper.MapEntityToDomainModel(employeeEntity);
        }

        public IEnumerable<Employee> GetAll()
        {
            IEnumerable<EmployeeEntity> employeeEntities = _employeeDAL.GetAll();
            return _employeeMapper.MapEntitiesToDomainModel(employeeEntities);
        }

        public IEnumerable<Employee> GetAllByAccountType(byte accountTypeId)
        {
            IEnumerable<EmployeeEntity> employeeEntities = _employeeDAL.GetAllByAccountType(accountTypeId);
            return _employeeMapper.MapEntitiesToDomainModel(employeeEntities);
        }

        public int Update(Employee employee)
        {
            EmployeeEntity employeeEntity = _employeeMapper.MapDomainModelToEntity(employee);
            return _employeeDAL.Update(employeeEntity);
        }
    }
}
