namespace Infrastructure.Entities
{
    public class EmployeeEntity : EntityBase
    {
        public ushort EmployeeId { get; set; }
        public ushort AccountId { get; set; }
        public byte DepartmentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ushort ManagerId { get; set; }
        public string MobileNumber { get; set; }
        public string NationalId { get; set; }
    }
}
