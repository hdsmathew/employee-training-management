using Assignment_v1.Common;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Assignment_v1.User
{
    public class UserDAL : IUserDAL
    {
        private readonly DbUtil _dbUtil;
        private readonly MapperBase<User> _userMapper;

        public UserDAL(DbUtil dbUtil, MapperBase<User> userMapper)
        {
            _dbUtil = dbUtil;
            _userMapper = userMapper;
        }

        public bool Add(User user)
        {
            string insertQuery = "INSERT INTO User (role, email, password, name, NIC, phone, deptID, managerID) " +
                                    "VALUES (@role, @email, @password, @name, @NIC, @phone, @deptID, @managerID)";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@role", user.Role),
                new SqlParameter("@email", user.Email),
                new SqlParameter("@password", user.Password),
                new SqlParameter("@name", user.Name),
                new SqlParameter("@NIC", user.NIC),
                new SqlParameter("@phone", user.Phone),
                new SqlParameter("@deptID", user.DeptID),
                new SqlParameter("@managerID", user.ManagerID)
            };

            return _dbUtil.ModifyData(insertQuery, parameters);
        }

        public bool Delete(int userID)
        {
            string deleteQuery = "DELETE FROM User WHERE ID = @ID";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@ID", userID)
            };

            return _dbUtil.ModifyData(deleteQuery, parameters);
        }

        public User Get(int userID)
        {
            string selectQuery = "SELECT * FROM User WHERE ID = @ID";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@ID", userID)
            };
            Dictionary<string, object> row = _dbUtil.GetData(selectQuery, parameters).First();

            return _userMapper.MapRowToObject(row);
        }

        public IEnumerable<User> GetAll()
        {
            string selectQuery = "SELECT * FROM User";
            List<SqlParameter> parameters = new List<SqlParameter>();
            IEnumerable<Dictionary<string, object>> entityTable = _dbUtil.GetData(selectQuery, parameters);

            return _userMapper.MapTableToObjects(entityTable);
        }

        public bool Update(User user)
        {
            string updateQuery = "UPDATE User SET " +
                                    "role =  @role" +
                                    ", email =  @email" +
                                    ", password =  @password" +
                                    ", name =  @name" +
                                    ", NIC =  @NIC" +
                                    ", phone =  @phone" +
                                    ", deptID =  @deptID" +
                                    ", managerID = @managerID" +
                                    "WHERE ID = @ID";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@ID", user.ID),
                new SqlParameter("@role", user.Role),
                new SqlParameter("@email", user.Email),
                new SqlParameter("@password", user.Password),
                new SqlParameter("@name", user.Name),
                new SqlParameter("@NIC", user.NIC),
                new SqlParameter("@phone", user.Phone),
                new SqlParameter("@deptID", user.DeptID),
                new SqlParameter("@managerID", user.ManagerID)
            };

            return _dbUtil.ModifyData(updateQuery, parameters);
        }
    }
}
