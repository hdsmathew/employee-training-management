using Assignment_v1.Common;
using System.Data.SqlClient;

namespace Assignment_v1.Enrollment
{
    public class EnrollmentDAL : IEnrollmentDAL
    {
        private readonly DbUtil _dbUtil;
        private readonly MapperBase<Enrollment> _enrollmentMapper;

        public EnrollmentDAL(DbUtil dbUtil, MapperBase<Enrollment> enrollmentMapper)
        {
            _dbUtil = dbUtil;
            _enrollmentMapper = enrollmentMapper;
        }

        public bool Add(Enrollment enrollment)
        {
            string insertQuery = "INSERT INTO Enrollment (employeeID, trainingID, status, message, requestDate, responseDate) " +
                                    "VALUES (@employeeID, @trainingID, @status, @message, @requestDate, @responseDate)";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@employeeID", enrollment.EmployeeID),
                new SqlParameter("@trainingID", enrollment.TrainingID),
                new SqlParameter("@status", enrollment.Status),
                new SqlParameter("@message", enrollment.Message),
                new SqlParameter("@requestDate", enrollment.RequestDate),
                new SqlParameter("@responseDate", enrollment.ResponseDate)
            };

            return _dbUtil.ModifyData(insertQuery, parameters);
        }

        public bool Delete(int enrollmentID)
        {
            string deleteQuery = "DELETE FROM Enrollment WHERE ID = @ID";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@ID", enrollmentID)
            };

            return _dbUtil.ModifyData(deleteQuery, parameters);
        }

        public Enrollment Get(int enrollmentID)
        {
            string selectQuery = "SELECT * FROM Enrollment WHERE ID + @ID";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@ID", enrollmentID)
            };
            Dictionary<string, object> row = _dbUtil.GetData(selectQuery, parameters).First();

            return _enrollmentMapper.MapRowToObject(row);
        }

        public IEnumerable<Enrollment> GetAll()
        {
            string selectQuery = "SELECT * FROM Enrollment";
            List<SqlParameter> parameters = new List<SqlParameter>();
            IEnumerable<Dictionary<string, object>> entityTable = _dbUtil.GetData(selectQuery, parameters);

            return _enrollmentMapper.MapTableToObjects(entityTable);
        }

        public bool Update(Enrollment enrollment)
        {
            string updateQuery = "UPDATE Enrollment SET " +
                                    "employeeID = @employeeID" +
                                    ", trainingID = @trainingID" +
                                    ", status = @status" +
                                    ", message = @message" +
                                    ", requestDate = @requestDate" +
                                    ", responseDate = @responseDate" +
                                    "WHERE ID = @ID";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("ID", enrollment.ID),
                new SqlParameter("@employeeID", enrollment.EmployeeID),
                new SqlParameter("@trainingID", enrollment.TrainingID),
                new SqlParameter("@status", enrollment.Status),
                new SqlParameter("@message", enrollment.Message),
                new SqlParameter("@requestDate", enrollment.RequestDate),
                new SqlParameter("@responseDate", enrollment.ResponseDate)
            };

            return _dbUtil.ModifyData(updateQuery, parameters);
        }
    }
}
