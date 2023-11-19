namespace Assignment_v1.User
{
    internal interface IUserRepository
    {
        void Add(User user);
        void Delete(int userID);
        User Get(int userID);
        IEnumerable<User> GetAll();
        void Update(User user);
    }
}
