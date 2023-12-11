using Core.Domain.User;
using Infrastructure.Common;
using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public class UserMapper : MapperBase<User, UserEntity>
    {
        public override UserEntity MapDomainModelToEntity(User domainModel)
        {
            return new UserEntity
            {
                ID = domainModel.ID,
                Role = (int)domainModel.Role,
                Email = domainModel.Email,
                Password = domainModel.Password,
                Name = domainModel.Name,
                NIC = domainModel.NIC,
                Phone = domainModel.Phone,
                DeptID = domainModel.DeptID
            };
        }

        public override User MapEntityToDomainModel(UserEntity entity)
        {
            return new User
            {
                ID = entity.ID,
                Role = (UserRoleEnum)entity.Role,
                Email = entity.Email,
                Password = entity.Password,
                Name = entity.Name,
                NIC = entity.NIC,
                Phone = entity.Phone,
                DeptID = entity.DeptID
            };
        }

        public override UserEntity MapRowToEntity(Dictionary<string, object> row)
        {
            return new UserEntity
            {
                ID = Convert.ToInt32(row["ID"]),
                Role = Convert.ToInt32(row["role"]),
                Email = row["email"].ToString(),
                Password = row["password"].ToString(),
                Name = row["name"].ToString(),
                NIC = row["NIC"].ToString(),
                Phone = row["phone"].ToString(),
                DeptID = Convert.ToInt32(row["deptID"])
            };
        }
    }
}
