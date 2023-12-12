using Core.Domain.Enrollment;
using Infrastructure.Common;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.DAL
{
    public class EnrollmentDAL : IEnrollmentDAL
    {
        private readonly DbUtil _dbUtil;
        private readonly MapperBase<Enrollment, EnrollmentEntity> _enrollmentMapper;

        public EnrollmentDAL(DbUtil dbUtil, EnrollmentMapper enrollmentMapper)
        {
            _dbUtil = dbUtil;
            _enrollmentMapper = enrollmentMapper;
        }

        public int Add(EnrollmentEntity enrollment)
        {
            string insertQuery = "INSERT INTO tbl_enrollment (employeeID, trainingID, status, message, requestDate) " +
                                    "VALUES (@employeeID, @trainingID, @status, @message, GETDATE())";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@employeeID", enrollment.EmployeeID),
                new SqlParameter("@trainingID", enrollment.TrainingID),
                new SqlParameter("@status", enrollment.Status),
                new SqlParameter("@message", enrollment.Message)
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

        public bool Exists(int employeeID, int trainingID)
        {
            string selectQuery = "SELECT COUNT(*) FROM tbl_enrollment WHERE " +
                                    "employeeID = @employeeID AND " +
                                    "trainingID = @trainingID AND " +
                                    "status = @status";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@employeeID", employeeID),
                new SqlParameter("@trainingID", trainingID),
                new SqlParameter("@status", (int)EnrollmentStatusEnum.Pending)
            };
            object scalarObject = _dbUtil.ExecuteScalar(selectQuery, parameters);
            return IsValidScalarObject(scalarObject) && Convert.ToInt32(scalarObject) > 0;
        }

        public EnrollmentEntity Get(int enrollmentID)
        {
            string selectQuery = "SELECT * FROM tbl_enrollment WHERE ID + @ID";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@ID", enrollmentID)
            };
            Dictionary<string, object> row = _dbUtil.ExecuteReader(selectQuery, parameters).First();
            return _enrollmentMapper.MapRowToEntity(row);
        }

        public IEnumerable<EnrollmentEntity> GetAll()
        {
            string selectQuery = "SELECT * FROM tbl_enrollment";
            List<SqlParameter> parameters = new List<SqlParameter>();
            IEnumerable<Dictionary<string, object>> entityDicts = _dbUtil.ExecuteReader(selectQuery, parameters);
            return _enrollmentMapper.MapTableToEntities(entityDicts);
        }

        public int Update(EnrollmentEntity enrollment)
        {
            string updateQuery = "UPDATE tbl_enrollment SET " +
                                    "status = @status" +
                                    ", message = @message" +
                                    ", responseDate = GETDATE()" +
                                    "WHERE ID = @ID";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("ID", enrollment.ID),
                new SqlParameter("@status", enrollment.Status),
                new SqlParameter("@message", enrollment.Message),
            };
            return _dbUtil.ExecuteNonQuery(updateQuery, parameters);
        }

        private bool IsValidScalarObject(object scalarObject)
        {
            return scalarObject != null && scalarObject != DBNull.Value;
        }
    }
}
