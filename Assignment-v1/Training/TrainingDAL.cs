using Assignment_v1.Common;
using System.Data;
using System.Data.SqlClient;

namespace Assignment_v1.Training
{
    internal class TrainingDAL : ITrainingDAL
    {
        private readonly DbUtil _dbUtil;

        public TrainingDAL(DbUtil dbUtil)
        {
            _dbUtil = dbUtil;
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
            Training training = new Training()
            {
                ID = trainingID,
                Name = dataRow["name"].ToString(),
                PreferredDeptID = Convert.ToInt32(dataRow["preferredDeptID"]),
                SeatsAvailable = Convert.ToInt32(dataRow["seatsAvailable"]),
                RegistrationDeadline = Convert.ToDateTime(dataRow["registrationDeadline"])
            };

            return training;
        }

        public IEnumerable<Training> GetAll()
        {
            string selectQuery = "SELECT * FROM Training";
            List<SqlParameter> parameters = new List<SqlParameter>();

            DataTable dataTable = _dbUtil.GetData(selectQuery, parameters);
            List<Training> trainings = new List<Training>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                Training training = new Training()
                {
                    ID = Convert.ToInt32(dataRow["ID"]),
                    Name = dataRow["name"].ToString(),
                    PreferredDeptID = Convert.ToInt32(dataRow["preferredDeptID"]),
                    SeatsAvailable = Convert.ToInt32(dataRow["seatsAvailable"]),
                    RegistrationDeadline = Convert.ToDateTime(dataRow["registrationDeadline"])
                };
                trainings.Add(training);
            }

            return trainings;
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
