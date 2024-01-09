using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Domain
{
    public class Employee : IEntity
    {
        private readonly List<EmployeeUpload> _documentUploads = new List<EmployeeUpload>();
        private readonly List<Enrollment> _enrollments = new List<Enrollment>();

        public Employee() { }

        public Employee(short accountId, byte departmentId, string firstName, string lastName, short managerId, string mobileNumber, string nationalId)
        {
            AccountId = accountId;
            DepartmentId = departmentId;
            FirstName = firstName;
            LastName = lastName;
            ManagerId = managerId;
            MobileNumber = mobileNumber;
            NationalId = nationalId;
        }

        public short EmployeeId { get; set; }
        public short AccountId { get; set; }
        public byte DepartmentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public short ManagerId { get; set; }
        public string MobileNumber { get; set; }
        public string NationalId { get; set; }
        public IEnumerable<EmployeeUpload> EmployeeUploads => _documentUploads;
        public IEnumerable<Enrollment> Enrollments => _enrollments;

        public void ApproveEnrollment(Enrollment enrollment)
        {
            ProcessEnrollment(enrollment, e => e.Approve());
        }

        public void ConfirmEnrollment(Enrollment enrollment)
        {
            ProcessEnrollment(enrollment, e => e.Confirm());
        }

        public void ConfirmEnrollments(IEnumerable<Enrollment> enrollments)
        {
            foreach (Enrollment enrollment in enrollments)
            {
                ConfirmEnrollment(enrollment);
            }
        }

        public void DeclineEnrollment(Enrollment enrollment)
        {
            ProcessEnrollment(enrollment, e => e.Decline());
        }

        public void DeclineEnrollment(IEnumerable<Enrollment> enrollments)
        {
            foreach (Enrollment enrollment in enrollments)
            {
                DeclineEnrollment(enrollment);
            }
        }

        public string GetFullName()
        {
            return FirstName + " " + LastName;
        }

        public Enrollment RegisterForTraining(Training training)
        {
            if (training.RegistrationDeadline > DateTime.UtcNow)
            {
                throw new Exception("Registration deadline for the training has already passed.");
            }

            if (training.Prerequisites.Any() && !HasTrainingPrerequisites(training.Prerequisites))
            {
                throw new Exception("Employee does not have the required training prerequisites.");
            }

            return new Enrollment(ApprovalStatusEnum.Pending, EmployeeId, training.TrainingId);
        }

        public void SetEmployeeUploads(IEnumerable<EmployeeUpload> uploads)
        {
            _documentUploads.AddRange(uploads);
        }

        public void SetEnrollments(IEnumerable<Enrollment> enrollments)
        {
            _enrollments.AddRange(enrollments);
        }

        public EmployeeUpload UploadPrerequisite(byte prerequisiteId, string uploadPath)
        {
            if (_documentUploads.Exists(upload => upload.PrerequisiteId == prerequisiteId))
            {
                throw new Exception("Employee has already uploaded prerequisite.");
            }

            EmployeeUpload employeeUpload = new EmployeeUpload(EmployeeId, prerequisiteId, uploadPath);
            _documentUploads.Add(employeeUpload);
            return employeeUpload;
        }

        private bool HasTrainingPrerequisites(IEnumerable<Prerequisite> trainingPrerequisites)
        {
            foreach (Prerequisite prerequisite in trainingPrerequisites)
            {
                if (!_documentUploads.Exists(upload => upload.PrerequisiteId == prerequisite.PrerequisiteId))
                {
                    return false;
                }
            };

            return true;
        }

        private void ProcessEnrollment(Enrollment enrollment, Action<Enrollment> process)
        {
            process(enrollment);
            enrollment.ApproverAccountId = AccountId;
        }
    }
}
