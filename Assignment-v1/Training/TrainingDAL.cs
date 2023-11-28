using Assignment_v1.Common;
using System.Data;
using System.Data.SqlClient;

namespace Assignment_v1.Training
{
    internal class TrainingDAL : ITrainingDAL
    {
        private readonly DbUtil _dbUtil;
        private readonly MapperBase<Training> _trainingMapper;

        public TrainingDAL(DbUtil dbUtil, MapperBase<Training> trainingMapper)
        {
            _dbUtil = dbUtil;
            _trainingMapper = trainingMapper;
        }

        public bool Add(Training training)
        {
            string insertQuery = "INSERT INTO Training (name, preferredDeptID, seatsAvailable, registrationDeadline) " +
                                    "VALUES (@name, @preferredDeptID, @seatsAvailable, @registrationDeadline)";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@name", training.Name),
                new SqlParameter("@preferredDeptID", training.PreferredDeptID),
                new SqlParameter("@seatsAvailable", training.SeatsAvailable),
                new SqlParameter("@registrationeDeadline", training.RegistrationDeadline)
            };

            return _dbUtil.ModifyData(insertQuery, parameters);
        }

        public bool Delete(int trainingID)
        {
            string deleteQuery = "DELETE FROM Training WHERE ID = @ID";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@ID", trainingID)
            };

            return _dbUtil.ModifyData(deleteQuery, parameters);
        }

        public Training Get(int trainingID)
        {
            string selectQuery = "SELECT * FROM Training WHERE ID = @ID";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ID", trainingID)
            };
            DataRow dataRow = _dbUtil.GetData(selectQuery, parameters).Rows[0];

            return _trainingMapper.MapRowToObject(dataRow);
        }

        public IEnumerable<Training> GetAll()
        {
            string selectQuery = "SELECT * FROM Training";
            List<SqlParameter> parameters = new List<SqlParameter>();
            DataTable dataTable = _dbUtil.GetData(selectQuery, parameters);

            return _trainingMapper.MapTableToObjects(dataTable);
        }

        public bool Update(Training training)
        {
            string updateQuery = "UPDATE Training SET " +
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

            return _dbUtil.ModifyData(updateQuery, parameters);
        }
    }
}
