namespace Assignment_v1.User
{
    public interface IUserDAL
    {
        bool Add(User user);
        bool Delete(int userID);
        User Get(int userID);
        IEnumerable<User> GetAll();
        bool Update(User user);
    }
}
