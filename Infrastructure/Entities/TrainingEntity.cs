using System;

namespace Infrastructure.Entities
{
    public class TrainingEntity : IEntity
    {
        public short TrainingId { get; set; }
        public byte PreferredDepartmentId { get; set; }
        public DateTime RegistrationDeadline { get; set; }
        public short SeatsAvailable { get; set; }
        public string TrainingDescription { get; set; }
        public string TrainingName { get; set; }
    }
}
