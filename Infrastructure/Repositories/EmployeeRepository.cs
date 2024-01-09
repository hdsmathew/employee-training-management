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
        private readonly IEmployeeUploadDAL _employeeUploadDAL;
        private readonly IEnrollmentDAL _enrollmentDAL;
        private readonly MapperBase<Enrollment, EnrollmentModel> _enrollmentMapper;
        private readonly MapperBase<Employee, EmployeeModel> _employeeMapper;
        private readonly MapperBase<EmployeeUpload, EmployeeUploadModel> _employeeUploadMapper;

        public EmployeeRepository(IEmployeeDAL employeeDAL, IEmployeeUploadDAL employeeUploadDAL,
            EmployeeMapper employeeMapper, EmployeeUploadMapper employeeUploadMapper,
            EnrollmentMapper enrollmentMapper, IEnrollmentDAL enrollmentDAL)
        {
            _employeeDAL = employeeDAL;
            _employeeUploadDAL = employeeUploadDAL;
            _employeeMapper = employeeMapper;
            _employeeUploadMapper = employeeUploadMapper;
            _enrollmentMapper = enrollmentMapper;
            _enrollmentDAL = enrollmentDAL;
        }

        public int Add(Employee employee)
        {
            EmployeeModel employeeEntity = _employeeMapper.MapEntityToDataModel(employee);
            return _employeeDAL.Add(employeeEntity);
        }

        public int Delete(short employeeId)
        {
            return _employeeDAL.Delete(employeeId);
        }

        public bool ExistsByNationalIdOrMobileNumber(string mobileNumber, string nationalId)
        {
            return _employeeDAL.ExistsByNationalIdOrMobileNumber(mobileNumber, nationalId);
        }

        public Employee Get(short employeeId)
        {
            EmployeeModel employeeModel = _employeeDAL.Get(employeeId);
            return _employeeMapper.MapDataModelToEntity(employeeModel);
        }

        public Employee GetByAccountId(short accountId)
        {
            EmployeeModel employeeModel = _employeeDAL.GetByAccountId(accountId);
            return _employeeMapper.MapDataModelToEntity(employeeModel);
        }

        public Employee GetWithEmployeeUploads(short employeeId)
        {
            Employee employee = Get(employeeId);
            employee.SetEmployeeUploads(_employeeUploadMapper.MapDataModelsToEntities(_employeeUploadDAL.GetAllByEmployeeId(employeeId)));
            return employee;
        }

        public Employee GetWithEnrollmentsByApprovalStatus(short employeeId, IEnumerable<ApprovalStatusEnum> approvalStatusEnums)
        {
            Employee employee = Get(employeeId);
            employee.SetEnrollments(_enrollmentMapper.MapDataModelsToEntities(_enrollmentDAL.GetAllByEmployeeIdAndApprovalStatus(employeeId, approvalStatusEnums)));
            return employee;
        }

        public IEnumerable<Employee> GetAll()
        {
            IEnumerable<EmployeeModel> employeeModels = _employeeDAL.GetAll();
            return _employeeMapper.MapDataModelsToEntities(employeeModels);
        }

        public IEnumerable<Employee> GetAllByEmployeeIds(IEnumerable<short> employeeIds)
        {
            IEnumerable<EmployeeModel> employeeModels = _employeeDAL.GetAllByEmployeeIds(employeeIds);
            return _employeeMapper.MapDataModelsToEntities(employeeModels);
        }

        public IEnumerable<Employee> GetAllByAccountType(AccountTypeEnum accountType)
        {
            IEnumerable<EmployeeModel> employeeModels = _employeeDAL.GetAllByAccountType((byte)accountType);
            return _employeeMapper.MapDataModelsToEntities(employeeModels);
        }

        public int Update(Employee employee)
        {
            EmployeeModel employeeModel = _employeeMapper.MapEntityToDataModel(employee);
            return _employeeDAL.Update(employeeModel);
        }
    }
}
