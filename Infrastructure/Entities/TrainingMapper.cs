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

        public override TrainingEntity MapRowToEntity((string, object)[] entityValueTuples)
        {
            return new TrainingEntity
            {
                TrainingId = GetValueFromTuple<ushort>("TrainingId", entityValueTuples),
                PreferredDepartmentId = GetValueFromTuple<byte>("PreferredDepartmentId", entityValueTuples),
                RegistrationDeadline = GetValueFromTuple<DateTime>("RegistrationDeadline", entityValueTuples),
                SeatsAvailable = GetValueFromTuple<ushort>("SeatsAvailable", entityValueTuples),
                TrainingDescription = GetValueFromTuple<string>("TrainingDescription", entityValueTuples),
                TrainingName = GetValueFromTuple<string>("TrainingName", entityValueTuples),
            };
        }
    }
}
