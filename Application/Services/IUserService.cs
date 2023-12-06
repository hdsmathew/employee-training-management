using Core.Domain.User;

namespace Core.Application.Services
{
    public interface IUserService
    {
        User Login(int userID, string password);
        void Logout(User user);
        bool Register(User user);
        void Update(User user);
    }
}
