using Core.Domain;
using Infrastructure.Common;
using Infrastructure.DAL.Interfaces;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.DAL
{
    public class EnrollmentDAL : IEnrollmentDAL
    {
        private readonly DataAccess _dataAccess;
        private readonly MapperBase<Enrollment, EnrollmentEntity> _enrollmentMapper;

        public EnrollmentDAL(DataAccess dataAccess, EnrollmentMapper enrollmentMapper)
        {
            _dataAccess = dataAccess;
            _enrollmentMapper = enrollmentMapper;
        }

        public int Add(EnrollmentEntity enrollment)
        {
            string insertQuery = @"INSERT INTO Enrollment (ApprovalStatusId, ApproverAccountId, EmployeeId, RequestedAt, TrainingId)
                                   VALUES (@ApprovalStatusId, @ApproverAccountId, @EmployeeId, GETDATE(), @TrainingId)";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@ApprovalStatusId", enrollment.ApprovalStatusId),
                new SqlParameter("@ApproverAccountId", enrollment.ApproverAccountId),
                new SqlParameter("@EmployeeId", enrollment.EmployeeId),
                new SqlParameter("@TrainingId", enrollment.TrainingId)
            };
            return _dataAccess.ExecuteNonQuery(insertQuery, parameters);
        }

        public int Delete(int enrollmentId)
        {
            string deleteQuery = "DELETE FROM Enrollment WHERE EnrollmentId = @EnrollmentId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@EnrollmentId", enrollmentId)
            };
            return _dataAccess.ExecuteNonQuery(deleteQuery, parameters);
        }

        public bool Exists(int employeeId, int trainingId)
        {
            string selectQuery = @"SELECT COUNT(*) FROM Enrollment WHERE 
                                   EmployeeId = @EmployeeId AND 
                                   TrainingId = @TrainingId AND 
                                   ApprovalStatusId = @ApprovalStatusId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@EmployeeId", employeeId),
                new SqlParameter("@TrainingId", trainingId),
                new SqlParameter("@ApprovalStatusId", (byte)ApprovalStatusEnum.Pending)
            };
            object scalarObject = _dataAccess.ExecuteScalar(selectQuery, parameters);
            return IsValidScalarObject(scalarObject) && Convert.ToInt32(scalarObject) > 0;
        }

        public EnrollmentEntity Get(int enrollmentId)
        {
            string selectQuery = "SELECT * FROM Enrollment WHERE EnrollmentId + @EnrollmentId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@EnrollmentId", enrollmentId)
            };
            (string, object)[] entityValueTuples = _dataAccess.ExecuteReader(selectQuery, parameters).First();
            return _enrollmentMapper.MapRowToEntity(entityValueTuples);
        }

        public IEnumerable<EnrollmentEntity> GetAll()
        {
            string selectQuery = "SELECT * FROM Enrollment";
            List<SqlParameter> parameters = new List<SqlParameter>();
            IEnumerable<(string, object)[]> entityValueTuplesArrays = _dataAccess.ExecuteReader(selectQuery, parameters);
            return _enrollmentMapper.MapTableToEntities(entityValueTuplesArrays);
        }

        public int Update(EnrollmentEntity enrollment)
        {
            string updateQuery = @"UPDATE Enrollment SET 
                                   ApprovalStatusId = @ApprovalStatusId, 
                                   ApproverAccountId = @ApproverAccountId, 
                                   UpdatedAt = GETDATE()
                                   WHERE EnrollmentId = @EnrollmentId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@ApprovalStatusId", enrollment.ApprovalStatusId),
                new SqlParameter("@ApproverAccountId", enrollment.ApproverAccountId),
                new SqlParameter("@EnrollmentId", enrollment.EnrollmentId)
            };
            return _dataAccess.ExecuteNonQuery(updateQuery, parameters);
        }

        private bool IsValidScalarObject(object scalarObject)
        {
            return scalarObject != null && scalarObject != DBNull.Value;
        }
    }
}
