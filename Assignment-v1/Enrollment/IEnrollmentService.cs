namespace Assignment_v1.Enrollment
{
    internal interface IEnrollmentService
    {
        void Process(Enrollment enrollment);
        void Submit(Enrollment enrollment);
        void ValidateEnrollments();
    }
}
