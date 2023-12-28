using Core.Domain;
using Infrastructure.Common;

namespace Infrastructure.Entities
{
    public class EmployeeMapper : MapperBase<Employee, EmployeeEntity>
    {
        public override EmployeeEntity MapDomainModelToEntity(Employee domainModel)
        {
            return new EmployeeEntity
            {
                EmployeeId = domainModel.EmployeeId,
                AccountId = domainModel.Account.AccountId,
                DepartmentId = domainModel.DepartmentId,
                FirstName = domainModel.FirstName,
                LastName = domainModel.LastName,
                ManagerId = domainModel.Manager.EmployeeId,
                MobileNumber = domainModel.MobileNumber,
                NationalId = domainModel.NationalId
            };
        }

        public override Employee MapEntityToDomainModel(EmployeeEntity entity)
        {
            return new Employee
            {
                EmployeeId = entity.EmployeeId,
                DepartmentId = entity.DepartmentId,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                MobileNumber = entity.MobileNumber,
                NationalId = entity.NationalId
            };
        }

        public override EmployeeEntity MapRowToEntity((string, object)[] entityValueTuples)
        {
            return new EmployeeEntity
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
