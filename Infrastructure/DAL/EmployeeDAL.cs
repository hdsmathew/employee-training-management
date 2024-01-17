using Core.Application;
using Core.Domain;
using Infrastructure.Common;
using Infrastructure.DAL.Interfaces;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task AddAsync(EmployeeModel employee)
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

            try
            {
                await _dataAccess.ExecuteNonQueryAsync(insertQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }
        }

        public async Task DeleteAsync(int employeeId)
        {
            string deleteQuery = "DELETE FROM Employee WHERE EmployeeId = @EmployeeId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@EmployeeId", employeeId)
            };

            try
            {
                await _dataAccess.ExecuteNonQueryAsync(deleteQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }
        }

        public async Task<bool> ExistsByNationalIdOrMobileNumberAsync(string mobileNumber, string nationalId)
        {
            string selectQuery = @"SELECT TOP 1 1 FROM Employee WHERE 
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
                scalarObject = await _dataAccess.ExecuteScalarAsync(selectQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }

            int.TryParse(scalarObject?.ToString(), out int scalarValue);
            return scalarValue == 1;
        }

        public async Task<EmployeeModel> GetAsync(int employeeId)
        {
            string selectQuery = @"SELECT EmployeeId, AccountId, DepartmentId, FirstName, LastName, ManagerId, MobileNumber, NationalId
                                   FROM Employee WHERE EmployeeId = @EmployeeId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@EmployeeId", employeeId)
            };
            IEnumerable<(string, object)[]> entityValueTuplesArrays;

            try
            {
                entityValueTuplesArrays = await _dataAccess.ExecuteReaderAsync(selectQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }
            return _employeeMapper.MapRowToDataModel(entityValueTuplesArrays.FirstOrDefault());
        }

        public async Task<EmployeeModel> GetByAccountIdAsync(short accountId)
        {
            string selectQuery = @"SELECT EmployeeId, AccountId, DepartmentId, FirstName, LastName, ManagerId, MobileNumber, NationalId
                                   FROM Employee WHERE AccountId = @AccountId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@AccountId", accountId)
            };
            IEnumerable<(string, object)[]> entityValueTuplesArrays;

            try
            {
                entityValueTuplesArrays = await _dataAccess.ExecuteReaderAsync(selectQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }
            return _employeeMapper.MapRowToDataModel(entityValueTuplesArrays.FirstOrDefault());
        }

        public async Task<IEnumerable<EmployeeModel>> GetAllAsync()
        {
            string selectQuery = @"SELECT EmployeeId, AccountId, DepartmentId, FirstName, LastName, ManagerId, MobileNumber, NationalId
                                   FROM Employee";
            List<SqlParameter> parameters = new List<SqlParameter>();
            IEnumerable<(string, object)[]> entityValueTuplesArrays;

            try
            {
                entityValueTuplesArrays = await _dataAccess.ExecuteReaderAsync(selectQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }
            return _employeeMapper.MapTableToDataModels(entityValueTuplesArrays);
        }

        public async Task<IEnumerable<EmployeeModel>> GetAllByEmployeeIdsAsync(IEnumerable<short> employeeIds)
        {
            string selectQuery = $"SELECT EmployeeId, FirstName, LastName, MobileNumber, DepartmentId, ManagerId FROM Employee WHERE EmployeeId IN ({string.Join(", ", employeeIds.ToList())})";
            List<SqlParameter> parameters = new List<SqlParameter>();
            IEnumerable<(string, object)[]> entityValueTuplesArrays;

            try
            {
                entityValueTuplesArrays = await _dataAccess.ExecuteReaderAsync(selectQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }
            return _employeeMapper.MapTableToDataModels(entityValueTuplesArrays);
        }

        public async Task<IEnumerable<EmployeeModel>> GetAllByAccountTypeAsync(byte accountTypeId)
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
                entityValueTuplesArrays = await _dataAccess.ExecuteReaderAsync(selectQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }
            return _employeeMapper.MapTableToDataModels(entityValueTuplesArrays);
        }

        public async Task UpdateAsync(EmployeeModel employee)
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

            try
            {
                await _dataAccess.ExecuteNonQueryAsync(updateQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }
        }
    }
}
