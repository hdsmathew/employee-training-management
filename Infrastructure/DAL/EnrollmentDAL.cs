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
using System.Threading.Tasks;

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

        public async Task AddAsync(EnrollmentModel enrollment)
        {
            string insertQuery = @"
            DECLARE @ManagerId SMALLINT;
            SET @ManagerId = (SELECT ManagerId FROM Employee WHERE EmployeeId = @EmployeeId);
            
            INSERT INTO Enrollment (ApprovalStatusId, ApproverAccountId, EmployeeId, RequestedAt, TrainingId)
            VALUES (@ApprovalStatusId, @ManagerId, @EmployeeId, GETDATE(), @TrainingId)";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@ApprovalStatusId", enrollment.ApprovalStatusId),
                new SqlParameter("@EmployeeId", enrollment.EmployeeId),
                new SqlParameter("@TrainingId", enrollment.TrainingId)
            };

            try
            {
                await _dataAccess.ExecuteNonQuery(insertQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }
        }

        public async Task AddWithEmployeeUploadsAsync(EnrollmentModel enrollment, IEnumerable<EmployeeUploadModel> employeeUploads)
        {
            StringBuilder insertQuery = new StringBuilder(@"
            BEGIN TRY
                BEGIN TRANSACTION
                    DECLARE @ManagerId SMALLINT;
                    SET @ManagerId = (SELECT ManagerId FROM Employee WHERE EmployeeId = @EmployeeId);

                    INSERT INTO Enrollment (ApprovalStatusId, ApproverAccountId, EmployeeId, RequestedAt, TrainingId)
                    VALUES (@ApprovalStatusId, @ManagerId, @EmployeeId, GETDATE(), @TrainingId);

                    INSERT INTO EmployeeUpload (EmployeeId, PrerequisiteId, UploadedAt, UploadedFileName) VALUES ");
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@ApprovalStatusId", enrollment.ApprovalStatusId),
                new SqlParameter("@EmployeeId", enrollment.EmployeeId),
                new SqlParameter("@TrainingId", enrollment.TrainingId)
            };

            int parameterIndex = 0;
            foreach (EmployeeUploadModel employeeUpload in employeeUploads)
            {
                string employeeUploadValues = $"(@EmployeeId, @PrerequisiteId{parameterIndex}, GETDATE(), @UploadedFileName{parameterIndex}), ";
                insertQuery.Append(employeeUploadValues);

                parameters.Add(new SqlParameter($"@PrerequisiteId{parameterIndex}", employeeUpload.PrerequisiteId));
                parameters.Add(new SqlParameter($"@UploadedFileName{parameterIndex}", employeeUpload.UploadedFileName));

                parameterIndex++;
            }
            insertQuery.Length -= 2;
            insertQuery.Append(@";
                    COMMIT;
            END TRY
            BEGIN CATCH
                ROLLBACK;
                THROW;
            END CATCH");

            try
            {
                await _dataAccess.ExecuteNonQuery(insertQuery.ToString(), parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }
        }

        public async Task DeleteAsync(int enrollmentId)
        {
            string deleteQuery = "DELETE FROM Enrollment WHERE EnrollmentId = @EnrollmentId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@EnrollmentId", enrollmentId)
            };

            try
            {
                await _dataAccess.ExecuteNonQuery(deleteQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }
        }

        public async Task<bool> ExistsAsync(short employeeId, short trainingId)
        {
            string selectQuery = @"SELECT TOP 1 1 FROM Enrollment WHERE 
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
                scalarObject = await _dataAccess.ExecuteScalar(selectQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }

            int.TryParse(scalarObject?.ToString(), out int scalarValue);
            return scalarValue == 1;
        }

        public async Task<EnrollmentModel> GetAsync(int enrollmentId)
        {
            string selectQuery = @"SELECT EnrollmentId, ApprovalStatusId, ApproverAccountId, EmployeeId, RequestedAt, TrainingId
                                   FROM Enrollment WHERE EnrollmentId = @EnrollmentId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@EnrollmentId", enrollmentId)
            };
            IEnumerable<(string, object)[]> entityValueTuplesArrays;

            try
            {
                entityValueTuplesArrays = await _dataAccess.ExecuteReaderAsync(selectQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }
            return _enrollmentMapper.MapRowToDataModel(entityValueTuplesArrays.FirstOrDefault());
        }

        public async Task<IEnumerable<EnrollmentModel>> GetAllAsync()
        {
            string selectQuery = @"SELECT EnrollmentId, ApprovalStatusId, ApproverAccountId, EmployeeId, RequestedAt, TrainingId
                                   FROM Enrollment";
            List<SqlParameter> parameters = new List<SqlParameter>();
            IEnumerable<(string, object)[]> entityValueTuplesArrays;

            try
            {
                entityValueTuplesArrays = await _dataAccess.ExecuteReaderAsync(selectQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }
            return _enrollmentMapper.MapTableToDataModels(entityValueTuplesArrays);
        }

        public async Task<IEnumerable<EnrollmentModel>> GetAllByTrainingIdAndApprovalStatusAsync(short trainingId, IEnumerable<ApprovalStatusEnum> approvalStatusEnums)
        {
            string selectQuery = $@"SELECT enr.EnrollmentId, enr.ApprovalStatusId, enr.ApproverAccountId, enr.EmployeeId, enr.RequestedAt, enr.TrainingId
                                    FROM Enrollment enr
                                    INNER JOIN Training tra ON enr.TrainingId = tra.TrainingId
                                    INNER JOIN ApprovalStatus appr ON enr.ApprovalStatusId = appr.ApprovalStatusId
                                    WHERE tra.TrainingId = @TrainingId
                                    AND appr.StatusName IN ({string.Join(", ", approvalStatusEnums.Select(status => $"'{status}'"))})";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@TrainingId", trainingId)
            };
            IEnumerable<(string, object)[]> entityValueTuplesArrays;

            try
            {
                entityValueTuplesArrays = await _dataAccess.ExecuteReaderAsync(selectQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }
            return _enrollmentMapper.MapTableToDataModels(entityValueTuplesArrays);
        }

        public async Task<IEnumerable<EnrollmentModel>> GetAllByEmployeeIdAndApprovalStatusAsync(short employeeId, IEnumerable<ApprovalStatusEnum> approvalStatusEnums)
        {
            string selectQuery = $@"SELECT enr.EnrollmentId, enr.ApprovalStatusId, enr.ApproverAccountId, enr.EmployeeId, enr.RequestedAt, enr.TrainingId
                                    FROM Enrollment enr
                                    INNER JOIN Employee emp ON enr.EmployeeId = emp.EmployeeId
                                    INNER JOIN ApprovalStatus appr ON enr.ApprovalStatusId = appr.ApprovalStatusId
                                    WHERE emp.EmployeeId = @EmployeeId
                                    AND appr.StatusName IN ({string.Join(", ", approvalStatusEnums.Select(status => $"'{status}'"))})";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@EmployeeId", employeeId)
            };
            IEnumerable<(string, object)[]> entityValueTuplesArrays;

            try
            {
                entityValueTuplesArrays = await _dataAccess.ExecuteReaderAsync(selectQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }
            return _enrollmentMapper.MapTableToDataModels(entityValueTuplesArrays);
        }

        public async Task<IEnumerable<EnrollmentModel>> GetAllByManagerIdAndApprovalStatusAsync(short managerId, IEnumerable<ApprovalStatusEnum> approvalStatusEnums)
        {
            string selectQuery = $@"SELECT enr.EnrollmentId, enr.ApprovalStatusId, enr.ApproverAccountId, enr.EmployeeId, enr.RequestedAt, enr.TrainingId
                                    FROM Enrollment enr
                                    INNER JOIN Employee emp ON enr.EmployeeId = emp.EmployeeId
                                    INNER JOIN ApprovalStatus appr ON enr.ApprovalStatusId = appr.ApprovalStatusId
                                    WHERE emp.ManagerId = @ManagerId
                                    AND appr.StatusName IN ({string.Join(", ", approvalStatusEnums.Select(status => $"'{status}'"))})";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@ManagerId", managerId)
            };
            IEnumerable<(string, object)[]> entityValueTuplesArrays;

            try
            {
                entityValueTuplesArrays = await _dataAccess.ExecuteReaderAsync(selectQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }
            return _enrollmentMapper.MapTableToDataModels(entityValueTuplesArrays);
        }

        public async Task UpdateAsync(EnrollmentModel enrollment)
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

            try
            {
                await _dataAccess.ExecuteNonQuery(updateQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }
        }

        public async Task UpdateBatchAsync(IEnumerable<EnrollmentModel> enrollments)
        {
            StringBuilder updateQuery = new StringBuilder(@"
            BEGIN TRANSACTION
                BEGIN TRY
                     CREATE TABLE #EnrollmentUpdateTemp (
                        EnrollmentId INT,
                        ApprovalStatusId TINYINT,
                        ApproverAccountId SMALLINT
                     );

                     INSERT INTO #EnrollmentUpdateTemp (EnrollmentId, ApprovalStatusId, ApproverAccountId) VALUES
            ");
            List<SqlParameter> parameters = new List<SqlParameter>();

            int parameterIndex = 0;
            foreach (EnrollmentModel enrollment in enrollments)
            {
                string employeeUploadValues = $"(@EnrollmentId{parameterIndex}, @ApprovalStatusId{parameterIndex}, @ApproverAccountId{parameterIndex}), ";
                updateQuery.Append(employeeUploadValues);

                parameters.Add(new SqlParameter($"@EnrollmentId{parameterIndex}", enrollment.EnrollmentId));
                parameters.Add(new SqlParameter($"@ApprovalStatusId{parameterIndex}", enrollment.ApprovalStatusId));
                parameters.Add(new SqlParameter($"@ApproverAccountId{parameterIndex}", enrollment.ApproverAccountId));

                parameterIndex++;
            }
            updateQuery.Length -= 2;

            updateQuery.Append(@";
                    UPDATE enr
                    SET enr.ApprovalStatusId = enrTemp.ApprovalStatusId,
                        enr.ApproverAccountId = enrTemp.ApproverAccountId,
                        enr.UpdatedAt = GETDATE()
                    FROM Enrollment enr
                    INNER JOIN #EnrollmentUpdateTemp enrTemp
                    ON enr.EnrollmentId = enrTemp.EnrollmentId;

                    COMMIT;
            END TRY
            BEGIN CATCH
                ROLLBACK;
                THROW;
            END CATCH
            ");

            try
            {
                await _dataAccess.ExecuteNonQuery(updateQuery.ToString(), parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }
        }
    }
}
