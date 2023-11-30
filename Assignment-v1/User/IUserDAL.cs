using System.Collections.Generic;

namespace Assignment_v1.User
{
    public interface IUserDAL
    {
        int Add(User user);
        bool Delete(int userID);
        bool Exists(User user);
        User Get(int userID);
        IEnumerable<User> GetAll();
        bool Update(User user);
    }
}
