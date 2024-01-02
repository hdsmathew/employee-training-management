using Core.Domain;
using Infrastructure.Common;
using System;

namespace Infrastructure.Models
{
    public class TrainingMapper : MapperBase<Training, TrainingModel>
    {
        public override TrainingModel MapEntityToDataModel(Training entity)
        {
            return new TrainingModel
            {
                TrainingId = entity.TrainingId,
                PreferredDepartmentId = entity.PreferredDepartmentId,
                RegistrationDeadline = entity.RegistrationDeadline,
                SeatsAvailable = entity.SeatsAvailable,
                TrainingDescription = entity.TrainingDescription,
                TrainingName = entity.TrainingName,
            };
        }

        public override Training MapDataModelToEntity(TrainingModel model)
        {
            return new Training
            {
                TrainingId = model.TrainingId,
                PreferredDepartmentId = model.PreferredDepartmentId,
                RegistrationDeadline = model.RegistrationDeadline,
                SeatsAvailable = model.SeatsAvailable,
                TrainingDescription = model.TrainingDescription,
                TrainingName = model.TrainingName,
            };
        }

        public override TrainingModel MapRowToDataModel((string, object)[] entityValueTuples)
        {
            return new TrainingModel
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
