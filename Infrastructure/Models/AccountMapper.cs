using Core.Domain;
using Infrastructure.Common;

namespace Infrastructure.Models
{
    public class AccountMapper : MapperBase<Account, AccountModel>
    {
        public override AccountModel MapEntityToDataModel(Account entity)
        {
            if (entity is null) return null;

            return new AccountModel
            {
                AccountId = entity.AccountId,
                AccountTypeId = (byte)entity.AccountType,
                EmailAddress = entity.EmailAddress,
                PasswordHash = entity.PasswordHash,
            };
        }

        public override Account MapDataModelToEntity(AccountModel model)
        {
            if (model is null) return null;

            return new Account
            {
                AccountId = model.AccountId,
                AccountType = (AccountTypeEnum)model.AccountTypeId,
                EmailAddress = model.EmailAddress,
                PasswordHash = model.PasswordHash,
            };
        }

        public override AccountModel MapRowToDataModel((string, object)[] entityValueTuples)
        {
            return new AccountModel
            {
                AccountId = GetValueFromTuple<short>("AccountId", entityValueTuples),
                AccountTypeId = GetValueFromTuple<byte>("AccountTypeId", entityValueTuples),
                EmailAddress = GetValueFromTuple<string>("EmailAddress", entityValueTuples),
                PasswordHash = GetValueFromTuple<string>("PasswordHash", entityValueTuples),
            };
        }
    }
}
