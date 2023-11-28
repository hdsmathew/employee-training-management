using Assignment_v1.Common;
using System.Data;

namespace Assignment_v1.Training
{
    internal class TrainingMapper : MapperBase<Training>
    {
        public override Training MapRowToObject(DataRow dataRow)
        {
            Training training = new Training()
            {
                ID = Convert.ToInt32(dataRow["ID"]),
                Name = dataRow["name"].ToString(),
                PreferredDeptID = Convert.ToInt32(dataRow["preferredDeptID"]),
                SeatsAvailable = Convert.ToInt32(dataRow["seatsAvailable"]),
                RegistrationDeadline = Convert.ToDateTime(dataRow["registrationDeadline"])
            };

            return training;
        }
    }
}
