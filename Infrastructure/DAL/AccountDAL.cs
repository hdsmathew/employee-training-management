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
    public class AccountDAL : IAccountDAL
    {
        private readonly DataAccess _dataAccess;
        private readonly MapperBase<Account, AccountModel> _accountMapper;

        public AccountDAL(DataAccess dataAccess, AccountMapper accountMapper)
        {
            _dataAccess = dataAccess;
            _accountMapper = accountMapper;
        }

        public async Task AddAsync(AccountModel account)
        {
            string insertQuery = @"INSERT INTO Account (AccountTypeId, CreatedAt, EmailAddress, PasswordHash)
                                   VALUES (@AccountTypeId, GETDATE(), @EmailAddress, @PasswordHash);";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@AccountTypeId", account.AccountTypeId),
                new SqlParameter("@EmailAddress", account.EmailAddress),
                new SqlParameter("@PasswordHash", account.PasswordHash)
            };

            try
            {
                await _dataAccess.ExecuteNonQuery(insertQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }
        }

        public async Task AddWithEmployeeDetailsAsync(AccountModel account, EmployeeModel employee)
        {
            string insertQuery = @"
            BEGIN TRY
                BEGIN TRANSACTION
                    INSERT INTO Account (AccountTypeId, CreatedAt, EmailAddress, PasswordHash)
                    VALUES (@AccountTypeId, GETDATE(), @EmailAddress, @PasswordHash);

                    DECLARE @AccountId SMALLINT;
                    SET @AccountId = SCOPE_IDENTITY();

                    INSERT INTO Employee (AccountId, DepartmentId, FirstName, LastName, ManagerId, MobileNumber, NationalId)
                    VALUES (@AccountId, @DepartmentId, @FirstName, @LastName, @ManagerId, @MobileNumber, @NationalId);
                    COMMIT;
            END TRY
            BEGIN CATCH
                ROLLBACK;
                THROW;
            END CATCH";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@AccountTypeId", account.AccountTypeId),
                new SqlParameter("@EmailAddress", account.EmailAddress),
                new SqlParameter("@PasswordHash", account.PasswordHash),
                new SqlParameter("@DepartmentId", employee.DepartmentId),
                new SqlParameter("@FirstName", employee.FirstName),
                new SqlParameter("@LastName", employee.LastName),
                new SqlParameter("@ManagerId", employee.ManagerId),
                new SqlParameter("@MobileNumber", employee.MobileNumber),
                new SqlParameter("@NationalId", employee.NationalId)
            };

            try
            {
                await _dataAccess.ExecuteNonQuery(insertQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }
        }

        public async Task DeleteAsync(int accountId)
        {
            string deleteQuery = "UPDATE Account SET IsActive = 0 WHERE AccountId = @AccountId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@AccountId", accountId)
            };

            try
            {
                await _dataAccess.ExecuteNonQuery(deleteQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }
        }

        public async Task<bool> ExistsByEmailAddressAsync(string emailAddress)
        {
            string selectQuery = @"SELECT TOP 1 1 FROM Account WHERE 
                                   EmailAddress = @EmailAddress";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@EmailAddress", emailAddress),
            };
            object scalarObject;

            try
            {
                scalarObject = await _dataAccess.ExecuteScalar(selectQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }
            int.TryParse(scalarObject?.ToString(), out int scalarValue);
            return scalarValue == 1;
        }

        public async Task<AccountModel> GetAsync(int accountId)
        {
            string selectQuery = "SELECT AccountId, AccountTypeId, EmailAddress, PasswordHash FROM Account WHERE AccountId = @AccountId";
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
            return _accountMapper.MapRowToDataModel(entityValueTuplesArrays.FirstOrDefault());
        }

        public async Task<AccountModel> GetAsync(string emailAddress, string passwordHash)
        {
            string selectQuery = @"SELECT AccountId, AccountTypeId, EmailAddress FROM Account WHERE 
                                   EmailAddress = @EmailAddress AND 
                                   PasswordHash = @PasswordHash";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@EmailAddress", emailAddress),
                new SqlParameter("@PasswordHash", passwordHash)
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
            return _accountMapper.MapRowToDataModel(entityValueTuplesArrays.FirstOrDefault());
        }

        public async Task<short> GetAccountIdByEmailAddressAsync(string emailAddress)
        {
            string selectQuery = "SELECT AccountId FROM Account WHERE EmailAddress = @EmailAddress";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@EmailAddress", emailAddress)
            };
            object scalarObject;

            try
            {
                scalarObject = await _dataAccess.ExecuteScalar(selectQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }

            short.TryParse(scalarObject?.ToString(), out short scalarValue);
            return scalarValue;
        }

        public async Task<short> GetAccountIdByAccountTypeAsync(AccountTypeEnum accountType)
        {
            string selectQuery = @"SELECT AccountId 
                                   FROM Account acc 
                                   INNER JOIN AccountType accType
                                   ON acc.AccountTypeId = accType.AccountTypeId
                                   WHERE TypeName = @TypeName";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@TypeName", accountType)
            };
            object scalarObject;

            try
            {
                scalarObject = await _dataAccess.ExecuteScalar(selectQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }

            short.TryParse(scalarObject?.ToString(), out short scalarValue);
            return scalarValue;
        }

        public async Task<AccountModel> GetByEmailAddressAsync(string emailAddress)
        {
            string selectQuery = "SELECT AccountId, AccountTypeId, PasswordHash FROM Account WHERE EmailAddress = @EmailAddress";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@EmailAddress", emailAddress)
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
            return _accountMapper.MapRowToDataModel(entityValueTuplesArrays.FirstOrDefault());
        }
    }
}
