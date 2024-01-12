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
    public class TrainingDAL : ITrainingDAL
    {
        private readonly DataAccess _dataAccess;
        private readonly MapperBase<Training, TrainingModel> _trainingMapper;

        public TrainingDAL(DataAccess dataAccess, TrainingMapper trainingMapper)
        {
            _dataAccess = dataAccess;
            _trainingMapper = trainingMapper;
        }

        public async Task<int> AddAsync(TrainingModel training)
        {
            string insertQuery = @"INSERT INTO Training (CreatedAt, PreferredDepartmentId, RegistrationDeadline, SeatsAvailable, TrainingDescription, TrainingName) 
                                   VALUES (GETDATE(), @PreferredDepartmentId, @RegistrationDeadline, @SeatsAvailable, @TrainingDescription, @TrainingName)";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@PreferredDepartmentId", training.PreferredDepartmentId),
                new SqlParameter("@RegistrationDeadline", training.RegistrationDeadline),
                new SqlParameter("@SeatsAvailable", training.SeatsAvailable),
                new SqlParameter("@TrainingDescription", training.TrainingDescription),
                new SqlParameter("@TrainingName", training.TrainingName)
            };
            int rowsAffected;

            try
            {
                rowsAffected = await _dataAccess.ExecuteNonQuery(insertQuery, parameters);
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

        public async Task<int> AddWithPrerequisitesAsync(TrainingModel training, IEnumerable<PrerequisiteModel> prerequisites)
        {
            StringBuilder insertQuery = new StringBuilder();
            insertQuery.Append(@"
            BEGIN TRY
                BEGIN TRANSACTION;
                    INSERT INTO Training (CreatedAt, PreferredDepartmentId, RegistrationDeadline, SeatsAvailable, TrainingDescription, TrainingName) 
                    VALUES (GETDATE(), @PreferredDepartmentId, @RegistrationDeadline, @SeatsAvailable, @TrainingDescription, @TrainingName);

                    DECLARE @TrainingId SMALLINT;
                    SET @TrainingId = SCOPE_IDENTITY();

                    INSERT INTO TrainingPrerequisite (TrainingId, PrerequisiteId) VALUES ");
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@PreferredDepartmentId", training.PreferredDepartmentId),
                new SqlParameter("@RegistrationDeadline", training.RegistrationDeadline),
                new SqlParameter("@SeatsAvailable", training.SeatsAvailable),
                new SqlParameter("@TrainingDescription", training.TrainingDescription),
                new SqlParameter("@TrainingName", training.TrainingName)
            };

            int parameterIndex = 0;
            foreach (PrerequisiteModel prerequisite in prerequisites)
            {
                string trainingPrerequisiteValues = $"(@TrainingId, @PrerequisiteId{parameterIndex}), ";
                insertQuery.Append(trainingPrerequisiteValues);

                parameters.Add(new SqlParameter($"@PrerequisiteId{parameterIndex}", prerequisite.PrerequisiteId));

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
            int rowsAffected;

            try
            {
                rowsAffected = await _dataAccess.ExecuteNonQuery(insertQuery.ToString(), parameters);
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

        public async Task<int> DeleteAsync(short trainingId)
        {
            string deleteQuery = "UPDATE Training SET IsActive = 0, UpdatedAt = GETDATE() WHERE TrainingId = @TrainingId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@TrainingId", trainingId)
            };
            int rowsAffected;

            try
            {
                rowsAffected = await _dataAccess.ExecuteNonQuery(deleteQuery, parameters);
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

        public async Task<bool> ExistsByNameAsync(string trainingName)
        {
            string selectQuery = @"SELECT COUNT(*) FROM Training WHERE 
                                   TrainingName = @TrainingName";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@TrainingName", trainingName)
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
            return scalarValue > 0;
        }

        public async Task<TrainingModel> GetAsync(short trainingId)
        {
            string selectQuery = "SELECT * FROM Training WHERE TrainingId = @TrainingId";
            List<SqlParameter> parameters = new List<SqlParameter>
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

            if (entityValueTuplesArrays.Count() > 1)
            {
                throw new DALException("More than 1 rows returned");
            }
            return _trainingMapper.MapRowToDataModel(entityValueTuplesArrays.Single());
        }

        public async Task<IEnumerable<TrainingModel>> GetAllAsync()
        {
            string selectQuery = "SELECT * FROM Training WHERE IsActive = 1";
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

            return _trainingMapper.MapTableToDataModels(entityValueTuplesArrays);
        }

        public async Task<IEnumerable<TrainingModel>> GetAllByRegistrationDeadlineDueAsync(DateTime registrationDeadline)
        {
            string selectQuery = "SELECT * FROM Training WHERE IsActive = 1 AND RegistrationDeadline < GETDATE()";
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

            return _trainingMapper.MapTableToDataModels(entityValueTuplesArrays);
        }

        public async Task<bool> HasEnrollmentsAsync(short trainingId)
        {
            string selectQuery = @"SELECT COUNT(trainingId) 
                                   FROM Training t
                                   INNER JOIN Enrollment e ON t.TrainingId = e.TrainingId
                                   INNER JOIN ApprovalStatus a ON e.ApprovalStatusId = a.ApprovalStatusId
                                   WHERE TrainingId = @TrainingId
                                   AND ApprovalStatusId IN ('Pending', 'Approved')";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@TrainingId", trainingId)
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
            return scalarValue > 0;
        }

        public async Task<int> UpdateAsync(TrainingModel training)
        {
            string updateQuery = @"UPDATE Training SET 
                                   PreferredDepartmentId = @PreferredDepartmentId, 
                                   RegistrationDeadline = @RegistrationDeadline, 
                                   SeatsAvailable = @seatsAvailable, 
                                   TrainingDescription = @TrainingDescription, 
                                   TrainingName = @TrainingName,
                                   UpdatedAt = GETDATE()
                                   WHERE TrainingId = @TrainingId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@TrainingId", training.TrainingId),
                new SqlParameter("@PreferredDepartmentId", training.PreferredDepartmentId),
                new SqlParameter("@RegistrationDeadline", training.RegistrationDeadline),
                new SqlParameter("@SeatsAvailable", training.SeatsAvailable),
                new SqlParameter("@TrainingDescription", training.TrainingDescription),
                new SqlParameter("@TrainingName", training.TrainingName)
            };
            int rowsAffected;

            try
            {
                rowsAffected = await _dataAccess.ExecuteNonQuery(updateQuery, parameters);
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
