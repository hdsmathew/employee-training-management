using Core.Domain;
using Infrastructure.Models;
using System.Threading.Tasks;

namespace Infrastructure.DAL.Interfaces
{
    public interface IAccountDAL
    {
        Task AddAsync(AccountModel account);
        Task AddWithEmployeeDetailsAsync(AccountModel account, EmployeeModel employee);
        Task DeleteAsync(int accountId);
        Task<bool> ExistsByEmailAddressAsync(string emailAddress);
        Task<AccountModel> GetAsync(int accountId);
        Task<AccountModel> GetAsync(string emailAddress, string passwordHash);
        Task<short> GetAccountIdByAccountTypeAsync(AccountTypeEnum accountType);
        Task<short> GetAccountIdByEmailAddressAsync(string emailAddress);
        Task<AccountModel> GetByEmailAddressAsync(string emailAddress);
    }
}
