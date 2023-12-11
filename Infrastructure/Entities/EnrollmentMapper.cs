using Core.Domain.Enrollment;
using Infrastructure.Common;
using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public class EnrollmentMapper : MapperBase<Enrollment, EnrollmentEntity>
    {
        public override EnrollmentEntity MapDomainModelToEntity(Enrollment domainModel)
        {
            return new EnrollmentEntity
            {
                ID = domainModel.ID,
                EmployeeID = domainModel.EmployeeID,
                TrainingID = domainModel.TrainingID,
                Status = (int)domainModel.Status,
                Message = domainModel.Message,
                RequestDate = domainModel.RequestDate,
                ResponseDate = domainModel.ResponseDate
            };
        }

        public override Enrollment MapEntityToDomainModel(EnrollmentEntity entity)
        {
            return new Enrollment
            {
                ID = entity.ID,
                EmployeeID = entity.EmployeeID,
                TrainingID = entity.TrainingID,
                Status = (EnrollmentStatusEnum)entity.Status,
                Message = entity.Message,
                RequestDate = entity.RequestDate,
                ResponseDate = entity.ResponseDate
            };
        }

        public override EnrollmentEntity MapRowToEntity(Dictionary<string, object> row)
        {
            return new EnrollmentEntity
            {
                ID = Convert.ToInt32(row["ID"]),
                EmployeeID = Convert.ToInt32(row["employeeID"]),
                TrainingID = Convert.ToInt32(row["trainingID"]),
                Status = Convert.ToInt32(row["status"]),
                Message = row["message"].ToString(),
                RequestDate = Convert.ToDateTime(row["requestDate"]),
                ResponseDate = Convert.ToDateTime(row["responseDate"])
            };
        }
    }
}
