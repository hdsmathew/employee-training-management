using Core.Domain;
using System.Collections.Generic;

namespace Core.Application.Repositories
{
    public interface IEmployeeRepository
    {
        int Add(Employee employee);
        int Delete(short employeeId);
        bool ExistsByNationalIdOrMobileNumber(string mobileNumber, string nationalId);
        Employee Get(short employeeId);
        IEnumerable<Employee> GetAll();
        IEnumerable<Employee> GetAllByAccountType(AccountTypeEnum accountType);
        IEnumerable<Employee> GetAllByEmployeeIds(IEnumerable<short> employeeIds);
        Employee GetByAccountId(short accountId);
        Employee GetWithEmployeeUploads(short employeeId);
        Employee GetWithEnrollmentsByApprovalStatus(short employeeId, IEnumerable<ApprovalStatusEnum> approvalStatusEnums);
        int Update(Employee employee);
    }
}
