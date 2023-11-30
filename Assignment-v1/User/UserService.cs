using System;

namespace Assignment_v1.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User Login(int userID, string password)
        {
            User user = _userRepository.Get(userID);
            if (!IsCredentialsValid(user, password))
            {
                throw new Exception("Invalid user id or password.");
            }

            return user;
        }

        public void Logout(User user)
        {
            throw new NotImplementedException();
        }

        public int Register(User user)
        {
            if (_userRepository.Exists(user))
            {
                throw new Exception("User already exists.");
            }

            return _userRepository.Add(user);
        }

        public void Update(User user)
        {
            throw new NotImplementedException();
        }

        private bool IsCredentialsValid(User user, string password)
        {
            return user != null && user.Password.Equals(password);
        }
    }
}
