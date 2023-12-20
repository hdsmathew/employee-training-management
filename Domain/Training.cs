using System;

namespace Core.Domain
{
    public class Training
    {
        public ushort TrainingId { get; set; }
        public byte PreferredDepartmentId { get; set; }
        public DateTime RegistrationDeadline { get; set; }
        public ushort SeatsAvailable { get; set; }
        public string TrainingDescription { get; set; }
        public string TrainingName { get; set; }
    }
}
