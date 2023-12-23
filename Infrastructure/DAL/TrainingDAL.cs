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
    public class TrainingDAL : ITrainingDAL
    {
        private readonly DataAccess _dataAccess;
        private readonly MapperBase<Training, TrainingEntity> _trainingMapper;

        public TrainingDAL(DataAccess dataAccess, TrainingMapper trainingMapper)
        {
            _dataAccess = dataAccess;
            _trainingMapper = trainingMapper;
        }

        public int Add(TrainingEntity training)
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
            return _dataAccess.ExecuteNonQuery(insertQuery, parameters);
        }

        public int Delete(int trainingId)
        {
            string deleteQuery = "DELETE FROM Training WHERE TrainingId = @TrainingId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@TrainingId", trainingId)
            };
            return _dataAccess.ExecuteNonQuery(deleteQuery, parameters);
        }

        public bool ExistsByName(string trainingName)
        {
            string selectQuery = @"SELECT COUNT(*) FROM Training WHERE 
                                   TrainingName = @TrainingName";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@TrainingName", trainingName)
            };
            object scalarObject = _dataAccess.ExecuteScalar(selectQuery, parameters);
            return IsValidScalarObject(scalarObject) && Convert.ToInt32(scalarObject) > 0;
        }

        public TrainingEntity Get(int trainingId)
        {
            string selectQuery = "SELECT * FROM Training WHERE TrainingId = @TrainingId";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@TrainingId", trainingId)
            };
            (string, object)[] entityValueTuples = _dataAccess.ExecuteReader(selectQuery, parameters).First();
            return _trainingMapper.MapRowToEntity(entityValueTuples);
        }

        public IEnumerable<TrainingEntity> GetAll()
        {
            string selectQuery = "SELECT * FROM Training";
            List<SqlParameter> parameters = new List<SqlParameter>();
            IEnumerable<(string, object)[]> entityValueTuplesArrays = _dataAccess.ExecuteReader(selectQuery, parameters);
            return _trainingMapper.MapTableToEntities(entityValueTuplesArrays);
        }

        public int Update(TrainingEntity training)
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
            return _dataAccess.ExecuteNonQuery(updateQuery, parameters);
        }

        private bool IsValidScalarObject(object scalarObject)
        {
            return scalarObject != null && scalarObject != DBNull.Value;
        }
    }
}
