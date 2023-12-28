using Infrastructure.Entities;
using System.Collections.Generic;

namespace Infrastructure.DAL.Interfaces
{
    public interface IAccountDAL
    {
        int Add(AccountEntity account);
        int Delete(int accountId);
        bool ExistsByEmailAddress(string emailAddress);
        AccountEntity Get(int accountId);
        AccountEntity Get(string emailAddress, string passwordHash);
        short GetAccountIdByEmailAddress(string emailAddress);
        IEnumerable<AccountEntity> GetAll();
        int Update(AccountEntity account);
    }
}
