using Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Repositories
{
    public interface IEmployeeRepository
    {
        Task<int> Add(Employee employee);
        Task<int> Delete(short employeeId);
        Task<bool> ExistsByNationalIdOrMobileNumber(string mobileNumber, string nationalId);
        Task<Employee> GetAsync(short employeeId);
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<IEnumerable<Employee>> GetAllByAccountTypeAsync(AccountTypeEnum accountType);
        Task<IEnumerable<Employee>> GetAllByEmployeeIdsAsync(IEnumerable<short> employeeIds);
        Task<Employee> GetByAccountIdAsync(short accountId);
        Task<Employee> GetWithEmployeeUploadsAsync(short employeeId);
        Task<Employee> GetWithEnrollmentsByApprovalStatusAsync(short employeeId, IEnumerable<ApprovalStatusEnum> approvalStatusEnums);
        Task<int> Update(Employee employee);
    }
}
