using Core.Domain.Common;
using Core.Domain.User;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.DAL
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
            string insertQuery = "INSERT INTO tbl_user (role, email, password, name, NIC, phone, deptID, managerID) " +
                                    "VALUES (@role, @email, @password, @name, @NIC, @phone, @deptID, @managerID);";
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
            return _dbUtil.ExecuteNonQuery(insertQuery, parameters);
        }

        public int Delete(int userID)
        {
            string deleteQuery = "DELETE FROM tbl_user WHERE ID = @ID";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@ID", userID)
            };
            return _dbUtil.ExecuteNonQuery(deleteQuery, parameters);
        }

        public bool ExistsByEmail(string email)
        {
            string selectQuery = "SELECT COUNT(*) FROM tbl_user WHERE " +
                                    "email = @email";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@email", email)
            };
            object scalarObject = _dbUtil.ExecuteScalar(selectQuery, parameters);
            return IsValidScalarObject(scalarObject) && Convert.ToInt32(scalarObject) > 0;
        }

        public User Get(int userID)
        {
            string selectQuery = "SELECT * FROM tbl_user WHERE ID = @ID";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@ID", userID)
            };
            Dictionary<string, object> row = _dbUtil.ExecuteReader(selectQuery, parameters).First();
            return _userMapper.MapRowToObject(row);
        }

        public IEnumerable<User> GetAll()
        {
            string selectQuery = "SELECT * FROM tbl_user";
            List<SqlParameter> parameters = new List<SqlParameter>();
            IEnumerable<Dictionary<string, object>> entityDicts = _dbUtil.ExecuteReader(selectQuery, parameters);
            return _userMapper.MapTableToObjects(entityDicts);
        }

        public int Update(User user)
        {
            string updateQuery = "UPDATE tbl_user SET " +
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
            return _dbUtil.ExecuteNonQuery(updateQuery, parameters);
        }

        private bool IsValidScalarObject(object scalarObject)
        {
            return scalarObject != null && scalarObject != DBNull.Value;
        }
    }
}
