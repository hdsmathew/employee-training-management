using Core.Application.Repositories;
using Core.Domain;
using Infrastructure.Common;
using Infrastructure.DAL.Interfaces;
using Infrastructure.Models;

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

        public int Add(Account account)
        {
            AccountModel accountModel = _accountMapper.MapEntityToDataModel(account);
            return _accountDAL.Add(accountModel);
        }

        public int AddWithEmployeeDetails(Account account, Employee employee)
        {
            AccountModel accountModel = _accountMapper.MapEntityToDataModel(account);
            EmployeeModel employeeModel = _employeeMapper.MapEntityToDataModel(employee);
            return _accountDAL.AddWithEmployeeDetails(accountModel, employeeModel);
        }

        public bool ExistsByEmailAddress(string emailAddress)
        {
            return _accountDAL.ExistsByEmailAddress(emailAddress);
        }

        public Account Get(int accountId)
        {
            AccountModel accountModel = _accountDAL.Get(accountId);
            return _accountMapper.MapDataModelToEntity(accountModel);
        }

        public Account Get(string emailAddress, string passwordHash)
        {
            AccountModel accountModel = _accountDAL.Get(emailAddress, passwordHash);
            return _accountMapper.MapDataModelToEntity(accountModel);
        }

        public short GetAccountIdByEmailAddress(string emailAddress)
        {
            return _accountDAL.GetAccountIdByEmailAddress(emailAddress);
        }
    }
}
