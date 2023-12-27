using System;

namespace Infrastructure.Entities
{
    public class TrainingEntity : IEntity
    {
        public ushort TrainingId { get; set; }
        public byte PreferredDepartmentId { get; set; }
        public DateTime RegistrationDeadline { get; set; }
        public ushort SeatsAvailable { get; set; }
        public string TrainingDescription { get; set; }
        public string TrainingName { get; set; }
    }
}
