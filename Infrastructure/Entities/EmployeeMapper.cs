using Core.Domain;
using Infrastructure.Common;
using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public class EmployeeMapper : MapperBase<Employee, EmployeeEntity>
    {
        public override EmployeeEntity MapDomainModelToEntity(Employee domainModel)
        {
            return new EmployeeEntity
            {
                EmployeeId = domainModel.EmployeeId,
                AccountId = domainModel.AccountId,
                DepartmentId = domainModel.DepartmentId,
                FirstName = domainModel.FirstName,
                LastName = domainModel.LastName,
                ManagerId = domainModel.ManagerId,
                MobileNumber = domainModel.MobileNumber,
                NationalId = domainModel.NationalId
            };
        }

        public override Employee MapEntityToDomainModel(EmployeeEntity entity)
        {
            return new Employee
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

        public override EmployeeEntity MapRowToEntity(Dictionary<string, object> row)
        {
            return new EmployeeEntity
            {
                EmployeeId = Convert.ToUInt16(row["EmployeeId"]),
                AccountId = Convert.ToUInt16(row["AccountId"]),
                DepartmentId = Convert.ToByte(row["DepartmentId"]),
                FirstName = row["FirstName"].ToString(),
                LastName = row["LastName"].ToString(),
                ManagerId = Convert.ToUInt16(row["ManagerId"]),
                MobileNumber = row["MobileNumber"].ToString(),
                NationalId = row["NationalId"].ToString()
            };
        }
    }
}
