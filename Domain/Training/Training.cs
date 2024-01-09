using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Domain
{
    public class Training : IEntity
    {
        private readonly List<Prerequisite> _prerequisites = new List<Prerequisite>();

        public Training() { }

        public Training(byte preferredDepartmentId, DateTime registrationDeadline, short seatsAvailable, string trainingDescription, string trainingName)
        {
            PreferredDepartmentId = preferredDepartmentId;
            RegistrationDeadline = registrationDeadline;
            SeatsAvailable = seatsAvailable;
            TrainingDescription = trainingDescription;
            TrainingName = trainingName;
        }

        public short TrainingId { get; set; }
        public byte PreferredDepartmentId { get; set; }
        [DisplayFormat(DataFormatString = "{0:ddd, MMM dd, yyyy h:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime RegistrationDeadline { get; set; }
        public short SeatsAvailable { get; set; }
        public string TrainingDescription { get; set; }
        public IEnumerable<Prerequisite> Prerequisites => _prerequisites;
        public string TrainingName { get; set; }

        public void SetPrerequisites(IEnumerable<Prerequisite> prerequisites)
        {
            _prerequisites.AddRange(prerequisites);
        }
    }
}
