using Core.Domain;
using System.Collections.Generic;

namespace Core.Application.Repositories
{
    public interface IAccountRepository
    {
        int Add(Account account);
        bool ExistsByEmailAddress(string emailAddress);
        Account Get(int accountId);
        Account Get(string emailAddress, string passwordHash);
        ushort GetAccountIdByEmailAddress(string emailAddress);
    }
}
