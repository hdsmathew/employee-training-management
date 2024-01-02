using Infrastructure.Models;
using System.Collections.Generic;

namespace Infrastructure.DAL.Interfaces
{
    public interface IAccountDAL
    {
        int Add(AccountModel account);
        int Delete(int accountId);
        bool ExistsByEmailAddress(string emailAddress);
        AccountModel Get(int accountId);
        AccountModel Get(string emailAddress, string passwordHash);
        short GetAccountIdByEmailAddress(string emailAddress);
        IEnumerable<AccountModel> GetAll();
        int Update(AccountModel account);
    }
}
