using Core.Application.Models;
using Core.Domain.User;

namespace Core.Application.Services
{
    public interface IUserService
    {
        Response<User> Login(int userID, string password);
        Response<User> Register(User user);
        Response<User> Update(User user);
    }
}
