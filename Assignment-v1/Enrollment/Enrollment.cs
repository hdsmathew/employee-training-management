using System;

namespace Assignment_v1.Enrollment
{
    public class Enrollment
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public int TrainingID { get; set; }
        public EnrollmentStatusEnum Status { get; set; }
        public string Message { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime ResponseDate { get; set; }
    }
}
