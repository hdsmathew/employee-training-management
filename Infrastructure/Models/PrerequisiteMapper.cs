using Core.Domain;
using Infrastructure.Common;

namespace Infrastructure.Models
{
    public class PrerequisiteMapper : MapperBase<Prerequisite, PrerequisiteModel>
    {
        public override PrerequisiteModel MapEntityToDataModel(Prerequisite entity)
        {
            return new PrerequisiteModel
            {
                PrerequisiteId = entity.PrerequisiteId,
                DocumentName = entity.DocumentName
            };
        }

        public override Prerequisite MapDataModelToEntity(PrerequisiteModel model)
        {
            return new Prerequisite
            {
                PrerequisiteId = model.PrerequisiteId,
                DocumentName = model.DocumentName
            };
        }

        public override PrerequisiteModel MapRowToDataModel((string, object)[] entityValueTuples)
        {
            return new PrerequisiteModel
            {
                PrerequisiteId = GetValueFromTuple<byte>("PrerequisiteId", entityValueTuples),
                DocumentName = GetValueFromTuple<string>("DocumentName", entityValueTuples)
            };
        }
    }
}
