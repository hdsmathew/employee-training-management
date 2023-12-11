using Core.Domain.Training;
using Infrastructure.Common;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.DAL
{
    public class TrainingDAL : ITrainingDAL
    {
        private readonly DbUtil _dbUtil;
        private readonly MapperBase<Training, TrainingEntity> _trainingMapper;

        public TrainingDAL(DbUtil dbUtil, TrainingMapper trainingMapper)
        {
            _dbUtil = dbUtil;
            _trainingMapper = trainingMapper;
        }

        public int Add(TrainingEntity training)
        {
            string insertQuery = "INSERT INTO tbl_training (name, preferredDeptID, seatsAvailable, registrationDeadline) " +
                                    "VALUES (@name, @preferredDeptID, @seatsAvailable, @registrationDeadline)";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@name", training.Name),
                new SqlParameter("@preferredDeptID", training.PreferredDeptID),
                new SqlParameter("@seatsAvailable", training.SeatsAvailable),
                new SqlParameter("@registrationeDeadline", training.RegistrationDeadline)
            };
            return _dbUtil.ExecuteNonQuery(insertQuery, parameters);
        }

        public int Delete(int trainingID)
        {
            string deleteQuery = "DELETE FROM tbl_training WHERE ID = @ID";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@ID", trainingID)
            };
            return _dbUtil.ExecuteNonQuery(deleteQuery, parameters);
        }

        public bool ExistsByName(string name)
        {
            string selectQuery = "SELECT COUNT(*) FROM tbl_training WHERE " +
                                    "name = @name";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@name", name)
            };
            object scalarObject = _dbUtil.ExecuteScalar(selectQuery, parameters);
            return IsValidScalarObject(scalarObject) && Convert.ToInt32(scalarObject) > 0;
        }

        public TrainingEntity Get(int trainingID)
        {
            string selectQuery = "SELECT * FROM tbl_training WHERE ID = @ID";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ID", trainingID)
            };
            Dictionary<string, object> row = _dbUtil.ExecuteReader(selectQuery, parameters).First();
            return _trainingMapper.MapRowToEntity(row);
        }

        public IEnumerable<TrainingEntity> GetAll()
        {
            string selectQuery = "SELECT * FROM tbl_training";
            List<SqlParameter> parameters = new List<SqlParameter>();
            IEnumerable<Dictionary<string, object>> entityDicts = _dbUtil.ExecuteReader(selectQuery, parameters);
            return _trainingMapper.MapTableToEntities(entityDicts);
        }

        public int Update(TrainingEntity training)
        {
            string updateQuery = "UPDATE tbl_training SET " +
                                    "name = @name" +
                                    ", preferredDeptID = @preferredDeptID" +
                                    ", seatsAvailable = @seatsAvailable" +
                                    ", registrationDeadline = @registrationDeadline" +
                                    "WHERE ID = @ID";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("ID", training.ID),
                new SqlParameter("@name", training.Name),
                new SqlParameter("@preferredDeptID", training.PreferredDeptID),
                new SqlParameter("@seatsAvailable", training.SeatsAvailable),
                new SqlParameter("@registrationeDeadline", training.RegistrationDeadline)
            };
            return _dbUtil.ExecuteNonQuery(updateQuery, parameters);
        }

        private bool IsValidScalarObject(object scalarObject)
        {
            return scalarObject != null && scalarObject != DBNull.Value;
        }
    }
}
