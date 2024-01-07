using Core.Application.Models;
using Core.Domain;

namespace Core.Application.Services
{
    public interface IAccountService
    {
        ResponseModel<AuthenticatedUser> Authenticate(LoginViewModel model);
        ResponseModel<Account> Create(Account account);
    }
}
