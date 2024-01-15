using System;

namespace Core.Domain
{
    public class Enrollment : IEntity, IEquatable<Enrollment>
    {
        public Enrollment() { }

        public Enrollment(ApprovalStatusEnum approvalStatus, short employeeId, short trainingId)
        {
            ApprovalStatus = approvalStatus;
            EmployeeId = employeeId;
            RequestedAt = DateTime.UtcNow;
            TrainingId = trainingId;
        }

        public int EnrollmentId { get; set; }
        public ApprovalStatusEnum ApprovalStatus { get; set; }
        public short ApproverAccountId { get; set; }
        public short EmployeeId { get; set; }
        public DateTime RequestedAt { get; set; }
        public short TrainingId { get; set; }
        public DateTime UpdatedAt { get; set; }

        public void Approve()
        {
            if (ApprovalStatus != ApprovalStatusEnum.Pending)
            {
                throw new Exception("Enrollment status is not pending. Cannot approve.");
            }
            UpdateApprovalStatusEnum(ApprovalStatusEnum.Approved);
        }

        public void Decline()
        {
            if (ApprovalStatus == ApprovalStatusEnum.Declined)
            {
                throw new Exception("Enrollment status is already declined.");
            }
            UpdateApprovalStatusEnum(ApprovalStatusEnum.Declined);
        }

        public void Confirm()
        {
            if (ApprovalStatus != ApprovalStatusEnum.Approved)
            {
                throw new Exception("Enrollment status has not been approved yet.");
            }
            UpdateApprovalStatusEnum(ApprovalStatusEnum.Confirmed);
        }

        private void UpdateApprovalStatusEnum(ApprovalStatusEnum status)
        {
            ApprovalStatus = status;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool Equals(Enrollment other)
        {
            if (other is null)
            {
                return false;
            }

            return EnrollmentId == other.EnrollmentId;
        }

        public override bool Equals(object otherObj)
        {
            return Equals(otherObj as Enrollment);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                EnrollmentId
            );
        }
    }
}
