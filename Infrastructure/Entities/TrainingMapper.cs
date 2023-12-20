using Core.Domain;
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
                TrainingId = domainModel.TrainingId,
                PreferredDepartmentId = domainModel.PreferredDepartmentId,
                RegistrationDeadline = domainModel.RegistrationDeadline,
                SeatsAvailable = domainModel.SeatsAvailable,
                TrainingDescription = domainModel.TrainingDescription,
                TrainingName = domainModel.TrainingName,
            };
        }

        public override Training MapEntityToDomainModel(TrainingEntity entity)
        {
            return new Training
            {
                TrainingId = entity.TrainingId,
                PreferredDepartmentId = entity.PreferredDepartmentId,
                RegistrationDeadline = entity.RegistrationDeadline,
                SeatsAvailable = entity.SeatsAvailable,
                TrainingDescription = entity.TrainingDescription,
                TrainingName = entity.TrainingName,
            };
        }

        public override TrainingEntity MapRowToEntity(Dictionary<string, object> row)
        {
            return new TrainingEntity
            {
                TrainingId = Convert.ToUInt16(row["TrainingId"]),
                PreferredDepartmentId = Convert.ToByte(row["PreferredDepartmentId"]),
                RegistrationDeadline = Convert.ToDateTime(row["RegistrationDeadline"]),
                SeatsAvailable = Convert.ToUInt16(row["SeatsAvailable"]),
                TrainingDescription = row["TrainingDescription"].ToString(),
                TrainingName = row["TrainingName"].ToString(),
            };
        }
    }
}
