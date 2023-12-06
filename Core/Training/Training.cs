using System;

namespace Core.Domain.Training
{
    public class Training
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int PreferredDeptID { get; set; }
        public int SeatsAvailable { get; set; }
        public DateTime RegistrationDeadline { get; set; }
    }
}
