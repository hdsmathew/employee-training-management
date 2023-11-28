namespace Assignment_v1.Enrollment
{
    internal interface IEnrollmentRepository
    {
        bool Add(Enrollment enrollment);
        bool Delete(int enrollmentID);
        Enrollment Get(int enrollmentID);
        IEnumerable<Enrollment> GetAll();
        bool Update(Enrollment enrollment);
    }
}
