using Core.Application.Models;
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

        public Response<User> Login(int userID, string password)
        {
            Response<User> response = new Response<User>();
            try
            {
                response.Entity = _userRepository.Get(userID, password);
            }
            catch (Exception ex)
            {
                response.AddError(new Error()
                {
                    Message = "Unable to login. Try again later.",
                    Exception = ex
                });
            }
            return response;
        }

        public Response<User> Register(User user)
        {
            Response<User> response = new Response<User>();
            try
            {
                if (_userRepository.ExistsByEmail(user.Email))
                {
                    response.AddError(new Error()
                    {
                        Message = $"User with email: {user.Email} already exists."
                    });
                    return response;
                }
                response.AddedRows = _userRepository.Add(user);
            }
            catch (Exception ex)
            {
                response.AddError(new Error()
                {
                    Message = "Registration failed. Try again later.",
                    Exception = ex
                });
            }
            return response;
        }

        public Response<User> Update(User user)
        {
            Response<User> response = new Response<User>();
            try
            {
                response.UpdatedRows = _userRepository.Update(user);
            }
            catch (Exception ex)
            {
                response.AddError(new Error()
                {
                    Message = $"Unable to update user with Id: {user.ID}",
                    Exception = ex
                });
            }
            return response;
        }
    }
}
