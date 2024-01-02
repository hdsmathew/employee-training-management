namespace Core.Domain
{
    public class Department : IEntity
    {
        public byte DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
}
