namespace Assignment_v1.User
{
    public interface IUserService
    {
        void Login(User user);
        void Logout(User user);
        void Register(User user);
        void Update(User user);
    }
}
