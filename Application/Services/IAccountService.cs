using Core.Application.Models;
using Core.Domain;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    public interface IAccountService
    {
        Task<ResultT<AuthenticatedUser>> AuthenticateAsync(LoginViewModel model);
        Task<Result> CreateAsync(Account account);
    }
}
