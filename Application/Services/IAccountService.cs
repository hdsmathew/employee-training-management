using Core.Application.Models;
using Core.Domain;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    public interface IAccountService
    {
        Task<ResponseModel<AuthenticatedUser>> AuthenticateAsync(LoginViewModel model);
        Task<ResponseModel<Account>> CreateAsync(Account account);
    }
}
