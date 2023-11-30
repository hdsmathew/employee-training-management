using System.Collections.Generic;

namespace Assignment_v1.User
{
    public interface IUserRepository
    {
        int Add(User user);
        bool Delete(int userID);
        bool Exists(User user);
        User Get(int userID);
        IEnumerable<User> GetAll();
        bool Update(User user);
    }
}
