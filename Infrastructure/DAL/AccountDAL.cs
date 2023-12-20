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
        private readonly DbUtil _dbUtil;
        private readonly MapperBase<Account, AccountEntity> _accountMapper;

        public AccountDAL(DbUtil dbUtil, AccountMapper accountMapper)
        {
            _dbUtil = dbUtil;
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
            return _dbUtil.ExecuteNonQuery(insertQuery, parameters);
        }

        public int Delete(int accountId)
        {
            string deleteQuery = "UPDATE Account SET IsActive = 0 WHERE AccountId = @AccountId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@AccountId", accountId)
            };
            return _dbUtil.ExecuteNonQuery(deleteQuery, parameters);
        }

        public bool ExistsByEmailAddress(string emailAddress)
        {
            string selectQuery = @"SELECT COUNT(*) FROM Account WHERE 
                                   EmailAddress = @EmailAddress";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@EmailAddress", emailAddress),
            };
            object scalarObject = _dbUtil.ExecuteScalar(selectQuery, parameters);
            return IsValidScalarObject(scalarObject) && Convert.ToInt32(scalarObject) > 0;
        }

        public AccountEntity Get(int accountId)
        {
            string selectQuery = "SELECT AccountId, AccountTypeId, EmailAddress, PasswordHash FROM Account WHERE AccountId = @AccountId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@AccountId", accountId)
            };
            Dictionary<string, object> row = _dbUtil.ExecuteReader(selectQuery, parameters).First();
            return _accountMapper.MapRowToEntity(row);
        }

        public AccountEntity Get(string emailAddress, string passwordHash)
        {
            string selectQuery = @"SELECT AccountId, AccountTypeId, EmailAddress, PasswordHash FROM Account WHERE 
                                   EmailAddress = @EmailAddress AND 
                                   PasswordHash = @PasswordHash";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@EmailAddress", emailAddress),
                new SqlParameter("@PasswordHash", passwordHash)
            };
            Dictionary<string, object> row = _dbUtil.ExecuteReader(selectQuery, parameters).First();
            return _accountMapper.MapRowToEntity(row);
        }

        public ushort GetAccountIdByEmailAddress(string emailAddress)
        {
            string selectQuery = "SELECT AccountId FROM Account WHERE EmailAddress = @EmailAddress";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@EmailAddress", emailAddress)
            };
            object scalarObject = _dbUtil.ExecuteScalar(selectQuery, parameters);
            return Convert.ToUInt16(scalarObject);
        }

        public IEnumerable<AccountEntity> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public int Update(AccountEntity account)
        {
            throw new System.NotImplementedException();
        }

        private bool IsValidScalarObject(object scalarObject)
        {
            return scalarObject != null && scalarObject != DBNull.Value;
        }
    }
}
