using Core.Application.Repositories;
using Core.Domain;
using Infrastructure.Common;
using Infrastructure.DAL.Interfaces;
using Infrastructure.Models;
using System.Collections.Generic;

namespace Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IEmployeeDAL _employeeDAL;
        private readonly MapperBase<Employee, EmployeeModel> _employeeMapper;

        public EmployeeRepository(IEmployeeDAL employeeDAL, EmployeeMapper employeeMapper)
        {
            _employeeDAL = employeeDAL;
            _employeeMapper = employeeMapper;
        }

        public int Add(Employee employee)
        {
            EmployeeModel employeeEntity = _employeeMapper.MapEntityToDataModel(employee);
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
            EmployeeModel employeeEntity = _employeeDAL.Get(employeeId);
            return _employeeMapper.MapDataModelToEntity(employeeEntity);
        }

        public IEnumerable<Employee> GetAll()
        {
            IEnumerable<EmployeeModel> employeeEntities = _employeeDAL.GetAll();
            return _employeeMapper.MapDataModelsToEntities(employeeEntities);
        }

        public IEnumerable<Employee> GetAllByAccountType(byte accountTypeId)
        {
            IEnumerable<EmployeeModel> employeeEntities = _employeeDAL.GetAllByAccountType(accountTypeId);
            return _employeeMapper.MapDataModelsToEntities(employeeEntities);
        }

        public int Update(Employee employee)
        {
            EmployeeModel employeeEntity = _employeeMapper.MapEntityToDataModel(employee);
            return _employeeDAL.Update(employeeEntity);
        }
    }
}
