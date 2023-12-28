using Core.Domain;
using System.Collections.Generic;

namespace Core.Application.Repositories
{
    public interface IEmployeeRepository
    {
        int Add(Employee employee);
        int Delete(int employeeId);
        bool ExistsByNationalIdOrMobileNumber(string nationalId, string mobileNumber);
        Employee Get(int employeeId);
        IEnumerable<Employee> GetAll();
        IEnumerable<Employee> GetAllByAccountType(byte accountTypeId);
        int Update(Employee employee);
    }
}
