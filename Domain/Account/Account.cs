namespace Core.Domain
{
    public class Account : IEntity
    {
        public Account() { }

        public Account(short accountId, AccountTypeEnum accountType, string emailAddress, string passwordHash)
        {
            AccountId = accountId;
            AccountType = accountType;
            EmailAddress = emailAddress;
            PasswordHash = passwordHash;
        }

        public short AccountId { get; set; }
        public AccountTypeEnum AccountType { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
    }
}
