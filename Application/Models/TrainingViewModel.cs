using Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Application.Models
{
    public class TrainingViewModel
    {
        public short TrainingId { get; set; }
        [Required(ErrorMessage = "Preferred Department is required")]
        public byte PreferredDepartmentId { get; set; }
        [Required(ErrorMessage = "Registration Deadline is required")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format")]
        public DateTime RegistrationDeadline { get; set; }
        [Required(ErrorMessage = "Seats Available is required")]
        [Range(1, short.MaxValue, ErrorMessage = "Seats Available must be greater than 0")]
        public short SeatsAvailable { get; set; }
        public string TrainingDescription { get; set; }
        [Required(ErrorMessage = "Training Name is required")]
        public string TrainingName { get; set; }
        public IEnumerable<Department> Departments { get; set; }
        public IEnumerable<Prerequisite> Prerequisites { get; set; }
        public List<byte> SelectedPrerequisiteIds { get; set; }
    }
}
