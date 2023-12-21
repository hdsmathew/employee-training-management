using Core.Domain;
using Infrastructure.Common;
using Infrastructure.DAL.Interfaces;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.DAL
{
    public class EmployeeDAL : IEmployeeDAL
    {
        private readonly DataAccess _dataAccess;
        private readonly MapperBase<Employee, EmployeeEntity> _employeeMapper;

        public EmployeeDAL(DataAccess dataAccess, EmployeeMapper employeeMapper)
        {
            _dataAccess = dataAccess;
            _employeeMapper = employeeMapper;
        }

        public int Add(EmployeeEntity employee)
        {
            string insertQuery = @"INSERT INTO Employee (AccountId, DepartmentId, FirstName, LastName, ManagerId, MobileNumber, NationalId)
                                   VALUES (@AccountId, @DepartmentId, @FirstName, @LastName, @ManagerId, @MobileNumber, @NationalId);";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@AccountId", employee.AccountId),
                new SqlParameter("@DepartmentId", employee.DepartmentId),
                new SqlParameter("@FirstName", employee.FirstName),
                new SqlParameter("@LastName", employee.LastName),
                new SqlParameter("@ManagerId", employee.ManagerId),
                new SqlParameter("@MobileNumber", employee.MobileNumber),
                new SqlParameter("@NationalId", employee.NationalId)
            };
            return _dataAccess.ExecuteNonQuery(insertQuery, parameters);
        }

        public int Delete(int employeeId)
        {
            string deleteQuery = "DELETE FROM Employee WHERE EmployeeId = @EmployeeId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@EmployeeId", employeeId)
            };
            return _dataAccess.ExecuteNonQuery(deleteQuery, parameters);
        }

        public bool ExistsByNationalIdOrMobileNumber(string mobileNumber, string nationalId)
        {
            string selectQuery = @"SELECT COUNT(*) FROM Employee WHERE 
                                   MobileNumber = @MobileNumber AND 
                                   NationalId = @NationalId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@MobileNumber", mobileNumber),
                new SqlParameter("@NationalId", nationalId)
            };
            object scalarObject = _dataAccess.ExecuteScalar(selectQuery, parameters);
            return IsValidScalarObject(scalarObject) && Convert.ToInt32(scalarObject) > 0;
        }

        public EmployeeEntity Get(int employeeId)
        {
            string selectQuery = "SELECT * FROM Employee WHERE EmployeeId = @EmployeeId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@EmployeeId", employeeId)
            };
            Dictionary<string, object> row = _dataAccess.ExecuteReader(selectQuery, parameters).First();
            return _employeeMapper.MapRowToEntity(row);
        }

        public IEnumerable<EmployeeEntity> GetAll()
        {
            string selectQuery = "SELECT * FROM Employee";
            List<SqlParameter> parameters = new List<SqlParameter>();
            IEnumerable<Dictionary<string, object>> entityDicts = _dataAccess.ExecuteReader(selectQuery, parameters);
            return _employeeMapper.MapTableToEntities(entityDicts);
        }

        public int Update(EmployeeEntity employee)
        {
            string updateQuery = @"UPDATE Employee SET 
                                   AccountId =  @AccountId, 
                                   DepartmentId =  @DepartmentId, 
                                   FirstName =  @FirstName, 
                                   LastName =  @LastName, 
                                   ManagerId =  @ManagerId, 
                                   MobileNumber =  @MobileNumber, 
                                   NationalId =  @NationalId, 
                                   WHERE EmployeeId = @EmployeeId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@EmployeeId", employee.EmployeeId),
                new SqlParameter("@AccountId", employee.AccountId),
                new SqlParameter("@DepartmentId", employee.DepartmentId),
                new SqlParameter("@FirstName", employee.FirstName),
                new SqlParameter("@LastName", employee.LastName),
                new SqlParameter("@ManagerId", employee.ManagerId),
                new SqlParameter("@MobileNumber", employee.MobileNumber),
                new SqlParameter("@NationalId", employee.NationalId)
            };
            return _dataAccess.ExecuteNonQuery(updateQuery, parameters);
        }

        private bool IsValidScalarObject(object scalarObject)
        {
            return scalarObject != null && scalarObject != DBNull.Value;
        }
    }
}
