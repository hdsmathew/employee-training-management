using Core.Domain.Training;
using Infrastructure.Common;
using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public class TrainingMapper : MapperBase<Training, TrainingEntity>
    {
        public override TrainingEntity MapDomainModelToEntity(Training domainModel)
        {
            return new TrainingEntity
            {
                ID = domainModel.ID,
                Name = domainModel.Name,
                PreferredDeptID = domainModel.PreferredDeptID,
                SeatsAvailable = domainModel.SeatsAvailable,
                RegistrationDeadline = domainModel.RegistrationDeadline
            };
        }

        public override Training MapEntityToDomainModel(TrainingEntity entity)
        {
            return new Training
            {
                ID = entity.ID,
                Name = entity.Name,
                PreferredDeptID = entity.PreferredDeptID,
                SeatsAvailable = entity.SeatsAvailable,
                RegistrationDeadline = entity.RegistrationDeadline
            };
        }

        public override TrainingEntity MapRowToEntity(Dictionary<string, object> row)
        {
            return new TrainingEntity
            {
                ID = Convert.ToInt32(row["ID"]),
                Name = row["name"].ToString(),
                PreferredDeptID = Convert.ToInt32(row["preferredDeptID"]),
                SeatsAvailable = Convert.ToInt32(row["seatsAvailable"]),
                RegistrationDeadline = Convert.ToDateTime(row["registrationDeadline"])
            };
        }
    }
}
