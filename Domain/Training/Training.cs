using System;
using System.Collections.Generic;

namespace Core.Domain
{
    public class Training
    {
        public short TrainingId { get; set; }
        public byte? PreferredDepartmentId { get; set; } = null;
        public DateTime RegistrationDeadline { get; set; }
        public short? SeatsAvailable { get; set; } = null;
        public string TrainingDescription { get; set; } = null;
        public IEnumerable<Prerequisite> Prerequisites { get; set; } = null;
        public string TrainingName { get; set; } = null;
    }
}
