using Core.Domain.User;
using System.Collections.Generic;

namespace Core.Application.Repositories
{
    public interface IUserRepository
    {
        bool Add(User user);
        bool Delete(int userID);
        bool Exists(User user);
        User Get(int userID);
        IEnumerable<User> GetAll();
        bool Update(User user);
    }
}
