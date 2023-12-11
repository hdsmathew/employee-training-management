using System;

namespace Infrastructure.Entities
{
    public class TrainingEntity : EntityBase
    {
        public string Name { get; set; }
        public int PreferredDeptID { get; set; }
        public int SeatsAvailable { get; set; }
        public DateTime RegistrationDeadline { get; set; }
    }
}
