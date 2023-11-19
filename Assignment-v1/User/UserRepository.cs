namespace Assignment_v1.User
{
    internal class UserRepository : IUserRepository
    {
        private readonly IUserDAL _userDAL;

        public UserRepository(IUserDAL userDAL)
        {
            _userDAL = userDAL;
        }

        public void Add(User user)
        {
            throw new NotImplementedException();
        }

        public void Delete(int userID)
        {
            throw new NotImplementedException();
        }

        public User Get(int userID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
