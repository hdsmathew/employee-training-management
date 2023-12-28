using System;
using System.Collections.Generic;

namespace Core.Domain
{
    public class Enrollment
    {
        public int? EnrollmentId { get; set; } = null;
        public ApprovalStatusEnum ApprovalStatus { get; set; }
        public short? ApproverAccountId { get; set; } = null;
        public DateTime RequestedAt { get; set; }
        public short? TrainingId { get; set; } = null;
        public DateTime UpdatedAt { get; set; }
        public IEnumerable<EnrollmentNotification> EnrollmentNotifications { get; set; } = null;
    }
}
