using Assignment_v1.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Assignment_v1.User
{
    public class UserDAL : IUserDAL
    {
        private readonly DbUtil _dbUtil;
        private readonly MapperBase<User> _userMapper;

        public UserDAL(DbUtil dbUtil, UserMapper userMapper)
        {
            _dbUtil = dbUtil;
            _userMapper = userMapper;
        }

        public int Add(User user)
        {
            string insertQuery = "INSERT INTO User (role, email, password, name, NIC, phone, deptID, managerID) " +
                                    "VALUES (@role, @email, @password, @name, @NIC, @phone, @deptID, @managerID); " +
                                    "SELECT SCOPE_IDENTITY()";
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

            return Convert.ToInt32(_dbUtil.ExecuteScalar(insertQuery, parameters));
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

        public bool Exists(User user)
        {
            string selectQuery = "SELECT COUNT(*) FROM User WHERE " +
                                    "email =  @email OR " +
                                    "NIC =  @NIC OR " +
                                    "phone =  @phone";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@email", user.Email),
                new SqlParameter("@NIC", user.NIC),
                new SqlParameter("@phone", user.Phone),
                new SqlParameter("@deptID", user.DeptID),
            };
            object scalarObject = _dbUtil.ExecuteScalar(selectQuery, parameters);

            return IsScalarObjectValid(scalarObject) && Convert.ToInt32(scalarObject) > 0;
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

        private bool IsScalarObjectValid(object scalarObject)
        {
            return scalarObject != null && scalarObject != DBNull.Value;
        }
    }
}
