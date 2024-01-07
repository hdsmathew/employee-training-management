using Core.Domain;
using System.Collections.Generic;

namespace Core.Application.Repositories
{
    public interface IEmployeeRepository
    {
        int Add(Employee employee);
        int Delete(short employeeId);
        bool ExistsByNationalIdOrMobileNumber(string nationalId, string mobileNumber);
        Employee Get(short employeeId);
        IEnumerable<Employee> GetAll();
        IEnumerable<Employee> GetAllByAccountType(AccountTypeEnum accountType);
        Employee GetWithEmployeeUploads(short employeeId);
        Employee GetWithEnrollmentsByApprovalStatus(short employeeId, IEnumerable<ApprovalStatusEnum> approvalStatusEnums);
        int Update(Employee employee);
    }
}
