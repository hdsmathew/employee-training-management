namespace Core.Domain
{
    public class Account
    {
        public short? AccountId { get; set; } = null;
        public AccountTypeEnum AccountType { get; set; }
        public string EmailAddress { get; set; } = null;
        public string PasswordHash { get; set; } = null;
    }
}
