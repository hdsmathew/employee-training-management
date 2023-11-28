using Assignment_v1.Common;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Assignment_v1.Enrollment
{
    internal class EnrollmentDAL : IEnrollmentDAL
    {
        private readonly DbUtil _dbUtil;

        public EnrollmentDAL(DbUtil dbUtil)
        {
            _dbUtil = dbUtil;
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

            DataRow dataRow = _dbUtil.GetData(selectQuery, parameters).Rows[0];
            Enrollment enrollment = new Enrollment()
            {
                ID = enrollmentID,
                EmployeeID = Convert.ToInt32(dataRow["employeeID"]),
                TrainingID = Convert.ToInt32(dataRow["trainingID"]),
                Status = (EnrollmentStatusEnum)dataRow["status"],
                Message = dataRow["message"].ToString(),
                RequestDate = Convert.ToDateTime(dataRow["requestDate"]),
                ResponseDate = Convert.ToDateTime(dataRow["responseDate"])
            };

            return enrollment;
        }

        public IEnumerable<Enrollment> GetAll()
        {
            string selectQuery = "SELECT * FROM Enrollment";
            List<SqlParameter> parameters = new List<SqlParameter>();

            DataTable dataTable = _dbUtil.GetData(selectQuery, parameters);
            List<Enrollment> enrollments = new List<Enrollment>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                Enrollment enrollment = new Enrollment()
                {
                    ID = enrollmentID,
                    EmployeeID = Convert.ToInt32(dataRow["employeeID"]),
                    TrainingID = Convert.ToInt32(dataRow["trainingID"]),
                    Status = (EnrollmentStatusEnum)dataRow["status"],
                    Message = dataRow["message"].ToString(),
                    RequestDate = Convert.ToDateTime(dataRow["requestDate"]),
                    ResponseDate = Convert.ToDateTime(dataRow["responseDate"])
                };

                enrollments.Add(enrollment);
            }

            return enrollments;
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
