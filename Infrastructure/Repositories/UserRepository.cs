using Core.Application.Repositories;
using Core.Domain.User;
using Infrastructure.Common;
using Infrastructure.DAL;
using Infrastructure.Entities;
using System.Collections.Generic;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IUserDAL _userDAL;
        private readonly MapperBase<User, UserEntity> _userMapper;

        public UserRepository(IUserDAL userDAL, UserMapper userMapper)
        {
            _userDAL = userDAL;
            _userMapper = userMapper;
        }

        public int Add(User user)
        {
            UserEntity userEntity = _userMapper.MapDomainModelToEntity(user);
            return _userDAL.Add(userEntity);
        }

        public int Delete(int userID)
        {
            return _userDAL.Delete(userID);
        }

        public bool ExistsByEmail(string email)
        {
            return _userDAL.ExistsByEmail(email);
        }

        public User Get(int userID)
        {
            UserEntity userEntity = _userDAL.Get(userID);
            return _userMapper.MapEntityToDomainModel(userEntity);
        }

        public User Get(int userID, string password)
        {
            UserEntity userEntity = _userDAL.Get(userID, password);
            return _userMapper.MapEntityToDomainModel(userEntity);
        }

        public IEnumerable<User> GetAll()
        {
            IEnumerable<UserEntity> userEntities = _userDAL.GetAll();
            return _userMapper.MapEntitiesToDomainModel(userEntities);
        }

        public int Update(User user)
        {
            UserEntity userEntity = _userMapper.MapDomainModelToEntity(user);
            return _userDAL.Update(userEntity);
        }
    }
}
