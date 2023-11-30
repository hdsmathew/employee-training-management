namespace Assignment_v1.User
{
    public interface IUserService
    {
        User Login(int userID, string password);
        void Logout(User user);
        int Register(User user);
        void Update(User user);
    }
}
