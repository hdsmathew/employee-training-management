using Core.Domain;
using Infrastructure.Common;

namespace Infrastructure.Entities
{
    public class PrerequisiteMapper : MapperBase<Prerequisite, PrerequisiteEntity>
    {
        public override PrerequisiteEntity MapDomainModelToEntity(Prerequisite domainModel)
        {
            return new PrerequisiteEntity
            {
                PrerequisiteId = domainModel.PrerequisiteId,
                DocumentName = domainModel.DocumentName
            };
        }

        public override Prerequisite MapEntityToDomainModel(PrerequisiteEntity entity)
        {
            return new Prerequisite
            {
                PrerequisiteId = entity.PrerequisiteId,
                DocumentName = entity.DocumentName
            };
        }

        public override PrerequisiteEntity MapRowToEntity((string, object)[] entityValueTuples)
        {
            return new PrerequisiteEntity
            {
                PrerequisiteId = GetValueFromTuple<byte>("PrerequisiteId", entityValueTuples),
                DocumentName = GetValueFromTuple<string>("DocumentName", entityValueTuples)
            };
        }
    }
}
