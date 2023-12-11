using System;

namespace Infrastructure.Entities
{
    public class EnrollmentEntity : EntityBase
    {
        public int EmployeeID { get; set; }
        public int TrainingID { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime ResponseDate { get; set; }
    }
}
