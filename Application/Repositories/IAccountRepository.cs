using Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Repositories
{
    public interface IAccountRepository
    {
        Task<int> Add(Account account);
        Task<int> AddWithEmployeeDetails(Account account, Employee employee);
        Task<bool> ExistsByEmailAddress(string emailAddress);
        Task<Account> GetAsync(int accountId);
        Task<Account> GetAsync(string emailAddress, string passwordHash);
        Task<short> GetAccountIdByAccountType(AccountTypeEnum accountType);
        Task<short> GetAccountIdByEmailAddress(string emailAddress);
        Task<Account> GetByEmailAddressAsync(string emailAddress);
    }
}
