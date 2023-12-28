namespace Infrastructure.Entities
{
    public class EmployeeEntity : IEntity
    {
        public short? EmployeeId { get; set; } = null;
        public short? AccountId { get; set; } = null;
        public byte? DepartmentId { get; set; } = null;
        public string FirstName { get; set; } = null;
        public string LastName { get; set; } = null;
        public short? ManagerId { get; set; } = null;
        public string MobileNumber { get; set; } = null;
        public string NationalId { get; set; } = null;
    }
}
