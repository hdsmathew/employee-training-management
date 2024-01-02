using Core.Domain;
using Infrastructure.Common;
using System;

namespace Infrastructure.Models
{
    public class EmployeeUploadMapper : MapperBase<EmployeeUpload, EmployeeUploadModel>
    {
        public override EmployeeUploadModel MapEntityToDataModel(EmployeeUpload entity)
        {
            return new EmployeeUploadModel()
            {
                PrerequisiteId = entity.PrerequisiteId,
                UploadedAt = entity.UploadedAt,
                UploadPath = entity.UploadPath
            };
        }

        public override EmployeeUpload MapDataModelToEntity(EmployeeUploadModel model)
        {
            return new EmployeeUpload()
            {
                UploadedAt = model.UploadedAt,
                UploadPath = model.UploadPath
            };
        }

        public override EmployeeUploadModel MapRowToDataModel((string, object)[] entityValueTuples)
        {
            return new EmployeeUploadModel()
            {
                PrerequisiteId = GetValueFromTuple<byte>("PrerequisiteId", entityValueTuples),
                UploadedAt = GetValueFromTuple<DateTime>("UploadedAt", entityValueTuples),
                UploadPath = GetValueFromTuple<string>("UploadPath", entityValueTuples)
            };
        }
    }
}
