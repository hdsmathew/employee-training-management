using Infrastructure.Entities;
using System.Collections.Generic;

namespace Infrastructure.DAL.Interfaces
{
    public interface IEmployeeDAL
    {
        int Add(EmployeeEntity user);
        int Delete(int userID);
        bool ExistsByNationalIdOrMobileNumber(string mobileNumber, string nationalId);
        EmployeeEntity Get(int userID);
        IEnumerable<EmployeeEntity> GetAll();
        IEnumerable<EmployeeEntity> GetEmployeesByAccountType(byte accountTypeId);
        int Update(EmployeeEntity user);
    }
}
