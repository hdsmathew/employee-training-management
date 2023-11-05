namespace Assignment_v1.User.User
{
    internal interface IUserService
    {
        void RegisterUser(User user);
        void LoginUser(User user);
        void LogoutUser(User user);
        void UpdateUser(User user);
    }
}
