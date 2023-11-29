namespace Assignment_v1.Enrollment
{
    public interface IEnrollmentService
    {
        void Process(Enrollment enrollment);
        void Submit(Enrollment enrollment);
        void ValidateEnrollments();
    }
}
