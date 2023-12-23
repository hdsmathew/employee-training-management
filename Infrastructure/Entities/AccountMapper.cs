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

        public override AccountEntity MapRowToEntity((string, object)[] entityValueTuples)
        {
            return new AccountEntity
            {
                AccountId = GetValueFromTuple<ushort>("AccountId", entityValueTuples),
                AccountTypeId = GetValueFromTuple<byte>("AccountTypeId", entityValueTuples),
                EmailAddress = GetValueFromTuple<string>("EmailAddress", entityValueTuples),
                PasswordHash = GetValueFromTuple<string>("PasswordHash", entityValueTuples),
            };
        }
    }
}
