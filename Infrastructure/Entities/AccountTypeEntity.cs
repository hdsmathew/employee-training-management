namespace Infrastructure.Entities
{
    public class AccountTypeEntity : IEntity
    {
        public byte AccountTypeId { get; set; }
        public string TypeName { get; set; }
    }
}
