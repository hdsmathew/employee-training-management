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
        int Update(EmployeeModel user);
    }
}
