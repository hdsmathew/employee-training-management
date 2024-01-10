using Core.Application.Repositories;
using Core.Domain;
using Infrastructure.Common;
using Infrastructure.DAL.Interfaces;
using Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public Task<int> Add(Employee employee)
        {
            EmployeeModel employeeEntity = _employeeMapper.MapEntityToDataModel(employee);
            return _employeeDAL.AddAsync(employeeEntity);
        }

        public Task<int> Delete(short employeeId)
        {
            return _employeeDAL.DeleteAsync(employeeId);
        }

        public Task<bool> ExistsByNationalIdOrMobileNumber(string mobileNumber, string nationalId)
        {
            return _employeeDAL.ExistsByNationalIdOrMobileNumberAsync(mobileNumber, nationalId);
        }

        public async Task<Employee> GetAsync(short employeeId)
        {
            EmployeeModel employeeModel = await _employeeDAL.GetAsync(employeeId);
            return _employeeMapper.MapDataModelToEntity(employeeModel);
        }

        public async Task<Employee> GetByAccountIdAsync(short accountId)
        {
            EmployeeModel employeeModel = await _employeeDAL.GetByAccountIdAsync(accountId);
            return _employeeMapper.MapDataModelToEntity(employeeModel);
        }

        public async Task<Employee> GetWithEmployeeUploadsAsync(short employeeId)
        {
            Employee employee = await GetAsync(employeeId);
            employee.SetEmployeeUploads(_employeeUploadMapper.MapDataModelsToEntities(await _employeeUploadDAL.GetAllByEmployeeIdAsync(employeeId)));
            return employee;
        }

        public async Task<Employee> GetWithEnrollmentsByApprovalStatusAsync(short employeeId, IEnumerable<ApprovalStatusEnum> approvalStatusEnums)
        {
            Employee employee = await GetAsync(employeeId);
            employee.SetEnrollments(_enrollmentMapper.MapDataModelsToEntities(await _enrollmentDAL.GetAllByEmployeeIdAndApprovalStatusAsync(employeeId, approvalStatusEnums)));
            return employee;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            IEnumerable<EmployeeModel> employeeModels = await _employeeDAL.GetAllAsync();
            return _employeeMapper.MapDataModelsToEntities(employeeModels);
        }

        public async Task<IEnumerable<Employee>> GetAllByEmployeeIdsAsync(IEnumerable<short> employeeIds)
        {
            IEnumerable<EmployeeModel> employeeModels = await _employeeDAL.GetAllByEmployeeIdsAsync(employeeIds);
            return _employeeMapper.MapDataModelsToEntities(employeeModels);
        }

        public async Task<IEnumerable<Employee>> GetAllByAccountTypeAsync(AccountTypeEnum accountType)
        {
            IEnumerable<EmployeeModel> employeeModels = await _employeeDAL.GetAllByAccountTypeAsync((byte)accountType);
            return _employeeMapper.MapDataModelsToEntities(employeeModels);
        }

        public Task<int> Update(Employee employee)
        {
            EmployeeModel employeeModel = _employeeMapper.MapEntityToDataModel(employee);
            return _employeeDAL.UpdateAsync(employeeModel);
        }
    }
}
