using Core.Domain;
using Infrastructure.Common;

namespace Infrastructure.Models
{
    public class PrerequisiteMapper : MapperBase<Prerequisite, PrerequisiteModel>
    {
        public override PrerequisiteModel MapEntityToDataModel(Prerequisite entity)
        {
            if (entity is null) return null;

            return new PrerequisiteModel
            {
                PrerequisiteId = entity.PrerequisiteId,
                DocumentName = entity.DocumentName
            };
        }

        public override Prerequisite MapDataModelToEntity(PrerequisiteModel model)
        {
            if (model is null) return null;

            return new Prerequisite
            {
                PrerequisiteId = model.PrerequisiteId,
                DocumentName = model.DocumentName
            };
        }

        public override PrerequisiteModel MapRowToDataModel((string, object)[] entityValueTuples)
        {
            if (entityValueTuples is null || entityValueTuples.Length == 0) return null;

            return new PrerequisiteModel
            {
                PrerequisiteId = GetValueFromTuple<byte>("PrerequisiteId", entityValueTuples),
                DocumentName = GetValueFromTuple<string>("DocumentName", entityValueTuples)
            };
        }
    }
}
