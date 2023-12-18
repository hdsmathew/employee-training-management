using Core.Domain.User;
using System.Collections.Generic;

namespace Core.Application.Repositories
{
    public interface IUserRepository
    {
        int Add(User user);
        int Delete(int userID);
        bool ExistsByEmail(string email);
        User Get(int userID);
        User Get(string emailAddress, string password);
        IEnumerable<User> GetAll();
        int Update(User user);
    }
}
