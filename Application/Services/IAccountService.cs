using Core.Application.Models;
using Core.Domain;

namespace Core.Application.Services
{
    public interface IAccountService
    {
        ResponseModel<Account> Authenticate(string emailAddress, string passwordHash);
        ResponseModel<Account> Create(Account account);
    }
}
