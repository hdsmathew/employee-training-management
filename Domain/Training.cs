using System;
using System.Collections.Generic;

namespace Core.Domain
{
    public class Training
    {
        public short TrainingId { get; set; }
        public byte PreferredDepartmentId { get; set; }
        public DateTime RegistrationDeadline { get; set; }
        public short SeatsAvailable { get; set; }
        public string TrainingDescription { get; set; }
        public IEnumerable<Prerequisite> Prerequisites { get; set; }
        public string TrainingName { get; set; }
    }
}
