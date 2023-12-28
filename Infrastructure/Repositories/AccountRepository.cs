using Core.Application.Repositories;
using Core.Domain;
using Infrastructure.Common;
using Infrastructure.DAL.Interfaces;
using Infrastructure.Entities;

namespace Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IAccountDAL _accountDAL;
        private readonly MapperBase<Account, AccountEntity> _accountMapper;

        public AccountRepository(IAccountDAL accountDAL, AccountMapper accountMapper)
        {
            _accountDAL = accountDAL;
            _accountMapper = accountMapper;
        }

        public int Add(Account account)
        {
            AccountEntity accountEntity = _accountMapper.MapDomainModelToEntity(account);
            return _accountDAL.Add(accountEntity);
        }

        public bool ExistsByEmailAddress(string emailAddress)
        {
            return _accountDAL.ExistsByEmailAddress(emailAddress);
        }

        public Account Get(int accountId)
        {
            AccountEntity accountEntity = _accountDAL.Get(accountId);
            return _accountMapper.MapEntityToDomainModel(accountEntity);
        }

        public Account Get(string emailAddress, string passwordHash)
        {
            AccountEntity accountEntity = _accountDAL.Get(emailAddress, passwordHash);
            return _accountMapper.MapEntityToDomainModel(accountEntity);
        }

        public short GetAccountIdByEmailAddress(string emailAddress)
        {
            return _accountDAL.GetAccountIdByEmailAddress(emailAddress);
        }
    }
}
