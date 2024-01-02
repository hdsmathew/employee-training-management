using Core.Application;
using Core.Domain;
using Infrastructure.Common;
using Infrastructure.DAL.Interfaces;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Infrastructure.DAL
{
    public class EnrollmentDAL : IEnrollmentDAL
    {
        private readonly DataAccess _dataAccess;
        private readonly MapperBase<Enrollment, EnrollmentModel> _enrollmentMapper;

        public EnrollmentDAL(DataAccess dataAccess, EnrollmentMapper enrollmentMapper)
        {
            _dataAccess = dataAccess;
            _enrollmentMapper = enrollmentMapper;
        }

        public int Add(EnrollmentModel enrollment)
        {
            string insertQuery = @"INSERT INTO Enrollment (ApprovalStatusId, EmployeeId, RequestedAt, TrainingId)
                                   VALUES (@ApprovalStatusId, @EmployeeId, GETDATE(), @TrainingId)";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@ApprovalStatusId", enrollment.ApprovalStatusId),
                new SqlParameter("@EmployeeId", enrollment.EmployeeId),
                new SqlParameter("@TrainingId", enrollment.TrainingId)
            };
            int rowsAffected;

            try
            {
                rowsAffected = _dataAccess.ExecuteNonQuery(insertQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }

            if (rowsAffected == 0)
            {
                throw new DALException("No rows added");
            }
            return rowsAffected;
        }

        public int AddWithEmployeeUploads(EnrollmentModel enrollment, IEnumerable<EmployeeUploadModel> employeeUploads)
        {
            StringBuilder insertQuery = new StringBuilder(@"INSERT INTO Enrollment (ApprovalStatusId, EmployeeId, RequestedAt, TrainingId)
                                                            VALUES (@ApprovalStatusId, @EmployeeId, GETDATE(), @TrainingId);
                                                            INSERT INTO EmployeeUpload (EmployeeId, PrerequisiteId, UploadPath) VALUES ");
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@ApprovalStatusId", enrollment.ApprovalStatusId),
                new SqlParameter("@EmployeeId", enrollment.EmployeeId),
                new SqlParameter("@TrainingId", enrollment.TrainingId)
            };
            int parameterIndex = 0;
            foreach (EmployeeUploadModel employeeUpload in employeeUploads)
            {
                string valueSet = $"(@EmployeeId, @PrerequisiteId{parameterIndex}, GETDATE(), @UploadPath{parameterIndex})";
                insertQuery.Append(valueSet);

                parameters.Add(new SqlParameter($"@PrerequisiteId{parameterIndex}", employeeUpload.PrerequisiteId));
                parameters.Add(new SqlParameter($"@UploadPath{parameterIndex}", employeeUpload.UploadPath));

                parameterIndex++;
                if (parameterIndex < employeeUploads.Count())
                {
                    insertQuery.Append(", ");
                }
            }
            int rowsAffected;

            try
            {
                rowsAffected = _dataAccess.ExecuteNonQuery(insertQuery.ToString(), parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }

            if (rowsAffected == 0)
            {
                throw new DALException("No rows added");
            }
            return rowsAffected;
        }

        public int Delete(int enrollmentId)
        {
            string deleteQuery = "DELETE FROM Enrollment WHERE EnrollmentId = @EnrollmentId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@EnrollmentId", enrollmentId)
            };
            int rowsAffected;

            try
            {
                rowsAffected = _dataAccess.ExecuteNonQuery(deleteQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }

            if (rowsAffected == 0)
            {
                throw new DALException("No rows deleted");
            }
            return rowsAffected;
        }

        public bool Exists(short employeeId, short trainingId)
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
            object scalarObject;

            try
            {
                scalarObject = _dataAccess.ExecuteScalar(selectQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }

            int.TryParse(scalarObject?.ToString(), out int scalarValue);
            return scalarValue > 0;
        }

        public EnrollmentModel Get(int enrollmentId)
        {
            string selectQuery = "SELECT * FROM Enrollment WHERE EnrollmentId + @EnrollmentId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@EnrollmentId", enrollmentId)
            };
            IEnumerable<(string, object)[]> entityValueTuplesArrays;

            try
            {
                entityValueTuplesArrays = _dataAccess.ExecuteReader(selectQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }

            if (entityValueTuplesArrays.Count() > 1)
            {
                throw new DALException("More than 1 rows returned");
            }
            return _enrollmentMapper.MapRowToDataModel(entityValueTuplesArrays.Single());
        }

        public IEnumerable<EnrollmentModel> GetAll()
        {
            string selectQuery = "SELECT * FROM Enrollment";
            List<SqlParameter> parameters = new List<SqlParameter>();
            IEnumerable<(string, object)[]> entityValueTuplesArrays;

            try
            {
                entityValueTuplesArrays = _dataAccess.ExecuteReader(selectQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }

            if (!entityValueTuplesArrays.Any())
            {
                throw new DALException("No rows returned");
            }
            return _enrollmentMapper.MapTableToEntities(entityValueTuplesArrays);
        }

        public int Update(EnrollmentModel enrollment)
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
            int rowsAffected;

            try
            {
                rowsAffected = _dataAccess.ExecuteNonQuery(updateQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }

            if (rowsAffected == 0)
            {
                throw new DALException("No rows updated");
            }
            return rowsAffected;
        }
    }
}
