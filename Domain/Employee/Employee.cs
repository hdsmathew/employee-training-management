using System.Collections.Generic;

namespace Core.Domain
{
    public class Employee
    {
        public short? EmployeeId { get; set; } = null;
        public Account Account { get; set; } = null;
        public byte? DepartmentId { get; set; } = null;
        public string FirstName { get; set; } = null;
        public string LastName { get; set; } = null;
        public Employee Manager { get; set; } = null;
        public string MobileNumber { get; set; } = null;
        public string NationalId { get; set; } = null;
        public IEnumerable<Enrollment> Enrollments { get; set; } = null;
        public IEnumerable<EmployeeUpload> EmployeeUploads { get; set; } = null;
    }
}
