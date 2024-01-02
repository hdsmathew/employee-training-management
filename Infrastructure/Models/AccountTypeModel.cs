namespace Infrastructure.Models
{
    public class AccountTypeModel : IModel
    {
        public byte AccountTypeId { get; set; }
        public string TypeName { get; set; }
    }
}
