using Infrastructure.Models;
using System.Collections.Generic;

namespace Infrastructure.DAL.Interfaces
{
    public interface IEmployeeDAL
    {
        int Add(EmployeeModel user);
        int Delete(int userID);
        bool ExistsByNationalIdOrMobileNumber(string mobileNumber, string nationalId);
        EmployeeModel Get(int userID);
        IEnumerable<EmployeeModel> GetAll();
        IEnumerable<EmployeeModel> GetAllByAccountType(byte accountTypeId);
        IEnumerable<EmployeeModel> GetAllByEmployeeIds(IEnumerable<short> employeeIds);
        EmployeeModel GetByAccountId(short accountId);
        int Update(EmployeeModel user);
    }
}
