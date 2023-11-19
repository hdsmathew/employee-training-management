namespace Assignment_v1.Enrollment
{
    internal class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly IEnrollmentDAL _enrollmentDAL;

        public EnrollmentRepository(IEnrollmentDAL enrollmentDAL)
        {
            _enrollmentDAL = enrollmentDAL;
        }

        public void Add(Enrollment enrollment)
        {
            throw new NotImplementedException();
        }

        public void Delete(int enrollmentID)
        {
            throw new NotImplementedException();
        }

        public Enrollment Get(int enrollmentID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Enrollment> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(Enrollment enrollment)
        {
            throw new NotImplementedException();
        }
    }
}
