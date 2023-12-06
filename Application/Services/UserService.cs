using Core.Application.Repositories;
using Core.Domain.User;
using System;

namespace Core.Application.Services
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
            if (IsAuthenticated(user, password))
            {
                return user;
            }
            throw new Exception("Invalid user id or password.");
        }

        public void Logout(User user)
        {
            throw new NotImplementedException();
        }

        public bool Register(User user)
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

        private bool IsAuthenticated(User user, string password)
        {
            return user != null && user.Password.Equals(password);
        }
    }
}
