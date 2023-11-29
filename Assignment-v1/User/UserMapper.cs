using Assignment_v1.Common;
using System;
using System.Collections.Generic;

namespace Assignment_v1.User
{
    public class UserMapper : MapperBase<User>
    {
        public override User MapRowToObject(Dictionary<string, object> row)
        {
            User user = new User
            {
                ID = Convert.ToInt32(row["ID"]),
                Role = (UserRoleEnum)row["role"],
                Email = row["email"].ToString(),
                Password = row["password"].ToString(),
                Name = row["name"].ToString(),
                NIC = row["NIC"].ToString(),
                Phone = row["phone"].ToString(),
                DeptID = Convert.ToInt32(row["deptID"])
            };

            return user;
        }
    }
}
