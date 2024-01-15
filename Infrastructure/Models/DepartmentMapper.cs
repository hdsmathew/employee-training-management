using Core.Domain;
using Infrastructure.Common;

namespace Infrastructure.Models
{
    public class DepartmentMapper : MapperBase<Department, DepartmentModel>
    {
        public override Department MapDataModelToEntity(DepartmentModel model)
        {
            if (model is null) return null;

            return new Department
            {
                DepartmentId = model.DepartmentId,
                DepartmentName = model.DepartmentName
            };
        }

        public override DepartmentModel MapEntityToDataModel(Department entity)
        {
            if (entity is null) return null;

            return new DepartmentModel
            {
                DepartmentId = entity.DepartmentId,
                DepartmentName = entity.DepartmentName
            };
        }

        public override DepartmentModel MapRowToDataModel((string, object)[] entityValueTuples)
        {
            return new DepartmentModel
            {
                DepartmentId = GetValueFromTuple<byte>("DepartmentId", entityValueTuples),
                DepartmentName = GetValueFromTuple<string>("DepartmentName", entityValueTuples)
            };
        }
    }
}
