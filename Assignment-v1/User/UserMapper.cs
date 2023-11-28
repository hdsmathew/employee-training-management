using Assignment_v1.Common;
using System.Data;

namespace Assignment_v1.User
{
    internal class UserMapper : MapperBase<User>
    {
        public override User MapRowToObject(DataRow dataRow)
        {
            User user = new User
            {
                ID = Convert.ToInt32(dataRow["ID"]),
                Role = (UserRoleEnum)dataRow["role"],
                Email = dataRow["email"].ToString(),
                Password = dataRow["password"].ToString(),
                Name = dataRow["name"].ToString(),
                NIC = dataRow["NIC"].ToString(),
                Phone = dataRow["phone"].ToString(),
                DeptID = Convert.ToInt32(dataRow["deptID"])
            };

            return user;
        }
    }
}
