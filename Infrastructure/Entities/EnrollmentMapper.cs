using Core.Domain;
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
                EnrollmentId = domainModel.EnrollmentId,
                ApprovalStatusId = domainModel.ApprovalStatusId,
                ApproverAccountId = domainModel.ApproverAccountId,
                EmployeeId = domainModel.EmployeeId,
                RequestedAt = domainModel.RequestedAt,
                TrainingId = domainModel.TrainingId,
                UpdatedAt = domainModel.UpdatedAt
            };
        }

        public override Enrollment MapEntityToDomainModel(EnrollmentEntity entity)
        {
            return new Enrollment
            {
                EnrollmentId = entity.EnrollmentId,
                ApprovalStatusId = entity.ApprovalStatusId,
                ApproverAccountId = entity.ApproverAccountId,
                EmployeeId = entity.EmployeeId,
                RequestedAt = entity.RequestedAt,
                TrainingId = entity.TrainingId,
                UpdatedAt = entity.UpdatedAt
            };
        }

        public override EnrollmentEntity MapRowToEntity(Dictionary<string, object> row)
        {
            return new EnrollmentEntity
            {
                EnrollmentId = Convert.ToUInt32(row["EnrollmentId"]),
                ApprovalStatusId = Convert.ToByte(row["ApprovalStatusId"]),
                ApproverAccountId = Convert.ToUInt16(row["ApproverAccountId"]),
                EmployeeId = Convert.ToUInt16(row["EmployeeId"]),
                RequestedAt = Convert.ToDateTime(row["RequestedAt"]),
                TrainingId = Convert.ToUInt16(row["TrainingId"]),
                UpdatedAt = Convert.ToDateTime(row["UpdatedAt"])
            };
        }
    }
}
