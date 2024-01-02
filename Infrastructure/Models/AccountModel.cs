namespace Infrastructure.Models
{
    public class AccountModel : IModel
    {
        public short AccountId { get; set; }
        public byte AccountTypeId { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
    }
}
