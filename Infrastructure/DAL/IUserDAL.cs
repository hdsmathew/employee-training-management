using Infrastructure.Entities;
using System.Collections.Generic;

namespace Infrastructure.DAL
{
    public interface IUserDAL
    {
        int Add(UserEntity user);
        int Delete(int userID);
        bool ExistsByEmail(string email);
        UserEntity Get(int userID);
        UserEntity Get(string emailAddress, string password);
        IEnumerable<UserEntity> GetAll();
        int Update(UserEntity user);
    }
}
