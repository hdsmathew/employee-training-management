using Core.Domain;
using System.Collections.Generic;

namespace Core.Application.Repositories
{
    public interface IEmployeeRepository
    {
        int Add(Employee user);
        int Delete(int userID);
        bool ExistsByNationalIdOrMobileNumber(string nationalId, string mobileNumber);
        Employee Get(int userID);
        IEnumerable<Employee> GetAll();
        int Update(Employee user);
    }
}
