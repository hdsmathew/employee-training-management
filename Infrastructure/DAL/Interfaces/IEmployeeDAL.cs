using Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.DAL.Interfaces
{
    public interface IEmployeeDAL
    {
        Task AddAsync(EmployeeModel user);
        Task DeleteAsync(int userID);
        Task<bool> ExistsByNationalIdOrMobileNumberAsync(string mobileNumber, string nationalId);
        Task<EmployeeModel> GetAsync(int userID);
        Task<IEnumerable<EmployeeModel>> GetAllAsync();
        Task<IEnumerable<EmployeeModel>> GetAllByAccountTypeAsync(byte accountTypeId);
        Task<IEnumerable<EmployeeModel>> GetAllByEmployeeIdsAsync(IEnumerable<short> employeeIds);
        Task<EmployeeModel> GetByAccountIdAsync(short accountId);
        Task UpdateAsync(EmployeeModel user);
    }
}
