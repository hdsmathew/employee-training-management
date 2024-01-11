using Core.Domain;
using Infrastructure.Common;

namespace Infrastructure.Models
{
    public class EmployeeMapper : MapperBase<Employee, EmployeeModel>
    {
        public override EmployeeModel MapEntityToDataModel(Employee entity)
        {
            if (entity is null) return null;

            return new EmployeeModel
            {
                EmployeeId = entity.EmployeeId,
                AccountId = entity.AccountId,
                DepartmentId = entity.DepartmentId,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                ManagerId = entity.ManagerId,
                MobileNumber = entity.MobileNumber,
                NationalId = entity.NationalId
            };
        }

        public override Employee MapDataModelToEntity(EmployeeModel model)
        {
            if (model is null) return null;

            return new Employee
            {
                EmployeeId = model.EmployeeId,
                AccountId = model.AccountId,
                DepartmentId = model.DepartmentId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                ManagerId = model.ManagerId,
                MobileNumber = model.MobileNumber,
                NationalId = model.NationalId
            };
        }

        public override EmployeeModel MapRowToDataModel((string, object)[] entityValueTuples)
        {
            if (entityValueTuples is null || entityValueTuples.Length == 0) return null;

            return new EmployeeModel
            {
                EmployeeId = GetValueFromTuple<short>("EmployeeId", entityValueTuples),
                AccountId = GetValueFromTuple<short>("AccountId", entityValueTuples),
                DepartmentId = GetValueFromTuple<byte>("DepartmentId", entityValueTuples),
                FirstName = GetValueFromTuple<string>("FirstName", entityValueTuples),
                LastName = GetValueFromTuple<string>("LastName", entityValueTuples),
                ManagerId = GetValueFromTuple<short>("ManagerId", entityValueTuples),
                MobileNumber = GetValueFromTuple<string>("MobileNumber", entityValueTuples),
                NationalId = GetValueFromTuple<string>("NationalId", entityValueTuples),
            };
        }
    }
}
