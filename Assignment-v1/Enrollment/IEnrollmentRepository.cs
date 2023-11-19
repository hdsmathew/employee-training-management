namespace Assignment_v1.Enrollment
{
    internal interface IEnrollmentRepository
    {
        void Add(Enrollment enrollment);
        void Delete(int enrollmentID);
        Enrollment Get(int enrollmentID);
        IEnumerable<Enrollment> GetAll();
        void Update(Enrollment enrollment);
    }
}
