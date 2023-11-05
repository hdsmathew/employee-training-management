namespace Assignment_v1.Enrollment
{
    internal interface IEnrollmentService
    {
        void SubmitEnrollment(Enrollment enrollment);
        void HandleEnrollment(Enrollment enrollment);
        void ValidateEnrollments();
    }
}
