using Core.Application;
using Core.Domain;
using Infrastructure.Common;
using Infrastructure.DAL.Interfaces;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.DAL
{
    public class EmployeeDAL : IEmployeeDAL
    {
        private readonly DataAccess _dataAccess;
        private readonly MapperBase<Employee, EmployeeModel> _employeeMapper;

        public EmployeeDAL(DataAccess dataAccess, EmployeeMapper employeeMapper)
        {
            _dataAccess = dataAccess;
            _employeeMapper = employeeMapper;
        }

        public int Add(EmployeeModel employee)
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
            int rowsAffected;

            try
            {
                rowsAffected = _dataAccess.ExecuteNonQuery(insertQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }

            if (rowsAffected == 0)
            {
                throw new DALException("No rows added");
            }
            return rowsAffected;
        }

        public int Delete(int employeeId)
        {
            string deleteQuery = "DELETE FROM Employee WHERE EmployeeId = @EmployeeId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@EmployeeId", employeeId)
            };
            int rowsAffected;

            try
            {
                rowsAffected = _dataAccess.ExecuteNonQuery(deleteQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }

            if (rowsAffected == 0)
            {
                throw new DALException("No rows deleted");
            }
            return rowsAffected;
        }

        public bool ExistsByNationalIdOrMobileNumber(string mobileNumber, string nationalId)
        {
            string selectQuery = @"SELECT COUNT(EmployeeId) FROM Employee WHERE 
                                   MobileNumber = @MobileNumber OR 
                                   NationalId = @NationalId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@MobileNumber", mobileNumber),
                new SqlParameter("@NationalId", nationalId)
            };
            object scalarObject;

            try
            {
                scalarObject = _dataAccess.ExecuteScalar(selectQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }

            int.TryParse(scalarObject?.ToString(), out int scalarValue);
            return scalarValue > 0;
        }

        public EmployeeModel Get(int employeeId)
        {
            string selectQuery = "SELECT * FROM Employee WHERE EmployeeId = @EmployeeId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@EmployeeId", employeeId)
            };
            IEnumerable<(string, object)[]> entityValueTuplesArrays;

            try
            {
                entityValueTuplesArrays = _dataAccess.ExecuteReader(selectQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }

            if (entityValueTuplesArrays.Count() > 1)
            {
                throw new DALException("More than 1 rows returned");
            }
            return _employeeMapper.MapRowToDataModel(entityValueTuplesArrays.Single());
        }

        public IEnumerable<EmployeeModel> GetAll()
        {
            string selectQuery = "SELECT * FROM Employee";
            List<SqlParameter> parameters = new List<SqlParameter>();
            IEnumerable<(string, object)[]> entityValueTuplesArrays;

            try
            {
                entityValueTuplesArrays = _dataAccess.ExecuteReader(selectQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }

            if (!entityValueTuplesArrays.Any())
            {
                throw new DALException("No rows returned");
            }
            return _employeeMapper.MapTableToDataModels(entityValueTuplesArrays);
        }

        public IEnumerable<EmployeeModel> GetAllByAccountType(byte accountTypeId)
        {
            string selectQuery = @"WITH AccountIds (AccountId) AS (
                                        SELECT AccountId FROM Account WHERE AccountTypeId = @AccountTypeId
                                   )
                                   SELECT EmployeeId, FirstName, LastName, DepartmentId
                                   FROM Employee emp INNER JOIN AccountIds accIds
                                   ON emp.AccountId = accIds.AccountId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@AccountTypeId", accountTypeId)
            };
            IEnumerable<(string, object)[]> entityValueTuplesArrays;

            try
            {
                entityValueTuplesArrays = _dataAccess.ExecuteReader(selectQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }

            return _employeeMapper.MapTableToDataModels(entityValueTuplesArrays);
        }

        public int Update(EmployeeModel employee)
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
            int rowsAffected;

            try
            {
                rowsAffected = _dataAccess.ExecuteNonQuery(updateQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }

            if (rowsAffected == 0)
            {
                throw new DALException("No rows updated");
            }
            return rowsAffected;
        }
    }
}
