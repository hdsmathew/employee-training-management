using Core.Domain;
using Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.DAL.Interfaces
{
    public interface IAccountDAL
    {
        Task<int> AddAsync(AccountModel account);
        Task<int> AddWithEmployeeDetailsAsync(AccountModel account, EmployeeModel employee);
        Task<int> DeleteAsync(int accountId);
        Task<bool> ExistsByEmailAddressAsync(string emailAddress);
        Task<AccountModel> GetAsync(int accountId);
        Task<AccountModel> GetAsync(string emailAddress, string passwordHash);
        Task<short> GetAccountIdByAccountTypeAsync(AccountTypeEnum accountType);
        Task<short> GetAccountIdByEmailAddressAsync(string emailAddress);
        Task<AccountModel> GetByEmailAddressAsync(string emailAddress);
    }
}
