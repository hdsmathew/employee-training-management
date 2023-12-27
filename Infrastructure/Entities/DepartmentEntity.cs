namespace Infrastructure.Entities
{
    public class DepartmentEntity : IEntity
    {
        public byte DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
}
