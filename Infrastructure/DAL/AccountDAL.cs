using Core.Application;
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
    public class AccountDAL : IAccountDAL
    {
        private readonly DataAccess _dataAccess;
        private readonly MapperBase<Account, AccountEntity> _accountMapper;

        public AccountDAL(DataAccess dataAccess, AccountMapper accountMapper)
        {
            _dataAccess = dataAccess;
            _accountMapper = accountMapper;
        }

        public int Add(AccountEntity account)
        {
            string insertQuery = @"INSERT INTO Account (AccountTypeId, CreatedAt, EmailAddress, PasswordHash)
                                   VALUES (@AccountTypeId, GETDATE(), @EmailAddress, @PasswordHash);";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@AccountTypeId", account.AccountTypeId),
                new SqlParameter("@EmailAddress", account.EmailAddress),
                new SqlParameter("@PasswordHash", account.PasswordHash)
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

        public int Delete(int accountId)
        {
            string deleteQuery = "UPDATE Account SET IsActive = 0 WHERE AccountId = @AccountId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@AccountId", accountId)
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

        public bool ExistsByEmailAddress(string emailAddress)
        {
            string selectQuery = @"SELECT COUNT(*) FROM Account WHERE 
                                   EmailAddress = @EmailAddress";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@EmailAddress", emailAddress),
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

        public AccountEntity Get(int accountId)
        {
            string selectQuery = "SELECT AccountId, AccountTypeId, EmailAddress, PasswordHash FROM Account WHERE AccountId = @AccountId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@AccountId", accountId)
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

            if (entityValueTuplesArrays.Count() < 1)
            {
                throw new DALException("No rows returned");
            }
            return _accountMapper.MapRowToEntity(entityValueTuplesArrays.Single());
        }

        public AccountEntity Get(string emailAddress, string passwordHash)
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
            return _accountMapper.MapRowToEntity(entityValueTuplesArrays.Single());
        }

        public ushort GetAccountIdByEmailAddress(string emailAddress)
        {
            string selectQuery = "SELECT AccountId FROM Account WHERE EmailAddress = @EmailAddress";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@EmailAddress", emailAddress)
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

            ushort.TryParse(scalarObject?.ToString(), out ushort scalarValue);
            return scalarValue;
        }

        public IEnumerable<AccountEntity> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public int Update(AccountEntity account)
        {
            throw new System.NotImplementedException();
        }
    }
}
