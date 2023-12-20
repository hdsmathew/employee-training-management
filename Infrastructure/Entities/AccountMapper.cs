using Core.Domain;
using Infrastructure.Common;
using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public class AccountMapper : MapperBase<Account, AccountEntity>
    {
        public override AccountEntity MapDomainModelToEntity(Account domainModel)
        {
            return new AccountEntity
            {
                AccountId = domainModel.AccountId,
                AccountTypeId = (byte)domainModel.AccountType,
                EmailAddress = domainModel.EmailAddress,
                PasswordHash = domainModel.PasswordHash,
            };
        }

        public override Account MapEntityToDomainModel(AccountEntity entity)
        {
            return new Account
            {
                AccountId = entity.AccountId,
                AccountType = (AccountTypeEnum)entity.AccountTypeId,
                EmailAddress = entity.EmailAddress,
                PasswordHash = entity.PasswordHash,
            };
        }

        public override AccountEntity MapRowToEntity(Dictionary<string, object> row)
        {
            return new AccountEntity
            {
                AccountId = Convert.ToUInt16(row["AccountId"]),
                AccountTypeId = Convert.ToByte(row["AccountTypeId"]),
                EmailAddress = row["EmailAddress"].ToString(),
                PasswordHash = row["PasswordHash"].ToString(),
            };
        }
    }
}
