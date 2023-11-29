namespace Assignment_v1.Enrollment
{
    public interface IEnrollmentDAL
    {
        bool Add(Enrollment enrollment);
        bool Delete(int enrollmentID);
        Enrollment Get(int enrollmentID);
        IEnumerable<Enrollment> GetAll();
        bool Update(Enrollment enrollment);
    }
}
