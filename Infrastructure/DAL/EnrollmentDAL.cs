using Core.Domain.Common;
using Core.Domain.Enrollment;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.DAL
{
    public class EnrollmentDAL : IEnrollmentDAL
    {
        private readonly DbUtil _dbUtil;
        private readonly MapperBase<Enrollment> _enrollmentMapper;

        public EnrollmentDAL(DbUtil dbUtil, EnrollmentMapper enrollmentMapper)
        {
            _dbUtil = dbUtil;
            _enrollmentMapper = enrollmentMapper;
        }

        public int Add(Enrollment enrollment)
        {
            string insertQuery = "INSERT INTO tbl_enrollment (employeeID, trainingID, status, message, requestDate, responseDate) " +
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
            return _dbUtil.ExecuteNonQuery(insertQuery, parameters);
        }

        public int Delete(int enrollmentID)
        {
            string deleteQuery = "DELETE FROM tbl_enrollment WHERE ID = @ID";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@ID", enrollmentID)
            };
            return _dbUtil.ExecuteNonQuery(deleteQuery, parameters);
        }

        public Enrollment Get(int enrollmentID)
        {
            string selectQuery = "SELECT * FROM tbl_enrollment WHERE ID + @ID";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@ID", enrollmentID)
            };
            Dictionary<string, object> row = _dbUtil.ExecuteReader(selectQuery, parameters).First();
            return _enrollmentMapper.MapRowToObject(row);
        }

        public IEnumerable<Enrollment> GetAll()
        {
            string selectQuery = "SELECT * FROM tbl_enrollment";
            List<SqlParameter> parameters = new List<SqlParameter>();
            IEnumerable<Dictionary<string, object>> entityDicts = _dbUtil.ExecuteReader(selectQuery, parameters);
            return _enrollmentMapper.MapTableToObjects(entityDicts);
        }

        public int Update(Enrollment enrollment)
        {
            string updateQuery = "UPDATE tbl_enrollment SET " +
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
            return _dbUtil.ExecuteNonQuery(updateQuery, parameters);
        }
    }
}
