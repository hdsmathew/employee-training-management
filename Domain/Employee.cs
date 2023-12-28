namespace Core.Domain
{
    public class Employee
    {
        public short EmployeeId { get; set; }
        public short AccountId { get; set; }
        public byte DepartmentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public short ManagerId { get; set; }
        public string MobileNumber { get; set; }
        public string NationalId { get; set; }
    }
}
