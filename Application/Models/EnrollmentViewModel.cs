using Core.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Application.Models
{
    public class EnrollmentViewModel
    {
        public EnrollmentViewModel() { }

        public EnrollmentViewModel(Enrollment enrollment)
        {
            EnrollmentId = enrollment.EnrollmentId;
            ApprovalStatus = enrollment.ApprovalStatus;
            RequestedAt = enrollment.RequestedAt;
        }

        public int EnrollmentId { get; set; }
        public ApprovalStatusEnum ApprovalStatus { get; set; }
        public string EmployeeName { get; set; }
        [DisplayFormat(DataFormatString = "{0:ddd, MMM dd, yyyy h:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime RequestedAt { get; set; }
        public string TrainingName { get; set; }
    }
}