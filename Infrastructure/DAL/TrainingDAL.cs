using Core.Application;
using Core.Domain;
using Infrastructure.Common;
using Infrastructure.DAL.Interfaces;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

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

        public int Add(TrainingModel training)
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

        public int Delete(int trainingId)
        {
            string deleteQuery = "DELETE FROM Training WHERE TrainingId = @TrainingId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@TrainingId", trainingId)
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

        public bool ExistsByName(string trainingName)
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
                scalarObject = _dataAccess.ExecuteScalar(selectQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }

            int.TryParse(scalarObject?.ToString(), out int scalarValue);
            return scalarValue > 0;
        }

        public TrainingModel Get(int trainingId)
        {
            string selectQuery = "SELECT * FROM Training WHERE TrainingId = @TrainingId";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@TrainingId", trainingId)
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
            return _trainingMapper.MapRowToDataModel(entityValueTuplesArrays.Single());
        }

        public IEnumerable<TrainingModel> GetAll()
        {
            string selectQuery = "SELECT * FROM Training";
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
            return _trainingMapper.MapTableToEntities(entityValueTuplesArrays);
        }

        public int Update(TrainingModel training)
        {
            string updateQuery = @"UPDATE Training SET 
                                   PreferredDepartmentId = @PreferredDepartmentId, 
                                   RegistrationDeadline = @RegistrationDeadline, 
                                   SeatsAvailable = @seatsAvailable, 
                                   TrainingDescription = @TrainingDescription, 
                                   TrainingName = @TrainingName,
                                   UpdatedAt = GETDATE(),
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
