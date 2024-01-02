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

        public AccountRepository(IAccountDAL accountDAL, AccountMapper accountMapper)
        {
            _accountDAL = accountDAL;
            _accountMapper = accountMapper;
        }

        public int Add(Account account)
        {
            AccountModel accountEntity = _accountMapper.MapEntityToDataModel(account);
            return _accountDAL.Add(accountEntity);
        }

        public bool ExistsByEmailAddress(string emailAddress)
        {
            return _accountDAL.ExistsByEmailAddress(emailAddress);
        }

        public Account Get(int accountId)
        {
            AccountModel accountEntity = _accountDAL.Get(accountId);
            return _accountMapper.MapDataModelToEntity(accountEntity);
        }

        public Account Get(string emailAddress, string passwordHash)
        {
            AccountModel accountEntity = _accountDAL.Get(emailAddress, passwordHash);
            return _accountMapper.MapDataModelToEntity(accountEntity);
        }

        public short GetAccountIdByEmailAddress(string emailAddress)
        {
            return _accountDAL.GetAccountIdByEmailAddress(emailAddress);
        }
    }
}
