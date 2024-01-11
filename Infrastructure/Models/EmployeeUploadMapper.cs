using Core.Domain;
using Infrastructure.Common;
using System;

namespace Infrastructure.Models
{
    public class EmployeeUploadMapper : MapperBase<EmployeeUpload, EmployeeUploadModel>
    {
        public override EmployeeUploadModel MapEntityToDataModel(EmployeeUpload entity)
        {
            if (entity is null) return null;

            return new EmployeeUploadModel()
            {
                PrerequisiteId = entity.PrerequisiteId,
                UploadedAt = entity.UploadedAt,
                UploadedFileName = entity.UploadedFileName
            };
        }

        public override EmployeeUpload MapDataModelToEntity(EmployeeUploadModel model)
        {
            if (model is null) return null;

            return new EmployeeUpload()
            {
                PrerequisiteId = model.PrerequisiteId,
                UploadedAt = model.UploadedAt,
                UploadedFileName = model.UploadedFileName
            };
        }

        public override EmployeeUploadModel MapRowToDataModel((string, object)[] entityValueTuples)
        {
            if (entityValueTuples is null || entityValueTuples.Length == 0) return null;

            return new EmployeeUploadModel()
            {
                PrerequisiteId = GetValueFromTuple<byte>("PrerequisiteId", entityValueTuples),
                UploadedAt = GetValueFromTuple<DateTime>("UploadedAt", entityValueTuples),
                UploadedFileName = GetValueFromTuple<string>("UploadedFileName", entityValueTuples)
            };
        }
    }
}
