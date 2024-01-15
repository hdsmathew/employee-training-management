using Core.Application.Repositories;
using Core.Domain;
using Infrastructure.Common;
using Infrastructure.DAL.Interfaces;
using Infrastructure.Models;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IAccountDAL _accountDAL;
        private readonly MapperBase<Account, AccountModel> _accountMapper;
        private readonly MapperBase<Employee, EmployeeModel> _employeeMapper;

        public AccountRepository(IAccountDAL accountDAL, AccountMapper accountMapper, EmployeeMapper employeeMapper)
        {
            _accountDAL = accountDAL;
            _accountMapper = accountMapper;
            _employeeMapper = employeeMapper;
        }

        public Task Add(Account account)
        {
            AccountModel accountModel = _accountMapper.MapEntityToDataModel(account);
            return _accountDAL.AddAsync(accountModel);
        }

        public Task AddWithEmployeeDetails(Account account, Employee employee)
        {
            if (account is null || employee is null) return null;

            AccountModel accountModel = _accountMapper.MapEntityToDataModel(account);
            EmployeeModel employeeModel = _employeeMapper.MapEntityToDataModel(employee);
            return _accountDAL.AddWithEmployeeDetailsAsync(accountModel, employeeModel);
        }

        public Task<bool> ExistsByEmailAddress(string emailAddress)
        {
            return _accountDAL.ExistsByEmailAddressAsync(emailAddress);
        }

        public async Task<Account> GetAsync(int accountId)
        {
            AccountModel accountModel = await _accountDAL.GetAsync(accountId);
            return _accountMapper.MapDataModelToEntity(accountModel);
        }

        public async Task<Account> GetAsync(string emailAddress, string passwordHash)
        {
            AccountModel accountModel = await _accountDAL.GetAsync(emailAddress, passwordHash);
            return _accountMapper.MapDataModelToEntity(accountModel);
        }

        public Task<short> GetAccountIdByEmailAddress(string emailAddress)
        {
            return _accountDAL.GetAccountIdByEmailAddressAsync(emailAddress);
        }

        public Task<short> GetAccountIdByAccountType(AccountTypeEnum accountType)
        {
            return _accountDAL.GetAccountIdByAccountTypeAsync(accountType);
        }

        public async Task<Account> GetByEmailAddressAsync(string emailAddress)
        {
            AccountModel accountModel = await _accountDAL.GetByEmailAddressAsync(emailAddress);
            return _accountMapper.MapDataModelToEntity(accountModel);
        }
    }
}
