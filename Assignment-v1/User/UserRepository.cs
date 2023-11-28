namespace Assignment_v1.User
{
    internal class UserRepository : IUserRepository
    {
        private readonly IUserDAL _userDAL;

        public UserRepository(IUserDAL userDAL)
        {
            _userDAL = userDAL;
        }

        public bool Add(User user)
        {
            return _userDAL.Add(user);
        }

        public bool Delete(int userID)
        {
            return _userDAL.Delete(userID);
        }

        public User Get(int userID)
        {
            return _userDAL.Get(userID);
        }

        public IEnumerable<User> GetAll()
        {
            return _userDAL.GetAll();
        }

        public bool Update(User user)
        {
            return _userDAL.Update(user);
        }
    }
}
