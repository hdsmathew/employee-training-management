using Core.Domain;
using Infrastructure.Common;
using System;

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
                TrainingId = entity.TrainingId ?? default,
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
                TrainingId = GetValueFromTuple<short>("TrainingId", entityValueTuples),
                PreferredDepartmentId = GetValueFromTuple<byte>("PreferredDepartmentId", entityValueTuples),
                RegistrationDeadline = GetValueFromTuple<DateTime>("RegistrationDeadline", entityValueTuples),
                SeatsAvailable = GetValueFromTuple<short>("SeatsAvailable", entityValueTuples),
                TrainingDescription = GetValueFromTuple<string>("TrainingDescription", entityValueTuples),
                TrainingName = GetValueFromTuple<string>("TrainingName", entityValueTuples),
            };
        }
    }
}
