using Core.Domain;
using Infrastructure.Common;
using System;

namespace Infrastructure.Entities
{
    public class EnrollmentMapper : MapperBase<Enrollment, EnrollmentEntity>
    {
        public override EnrollmentEntity MapDomainModelToEntity(Enrollment domainModel)
        {
            return new EnrollmentEntity
            {
                EnrollmentId = domainModel.EnrollmentId,
                ApprovalStatusId = (byte)domainModel.ApprovalStatus,
                ApproverAccountId = domainModel.ApproverAccountId,
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
                ApprovalStatus = (ApprovalStatusEnum)entity.ApprovalStatusId,
                ApproverAccountId = entity.ApproverAccountId,
                RequestedAt = entity.RequestedAt,
                TrainingId = entity.TrainingId,
                UpdatedAt = entity.UpdatedAt
            };
        }

        public override EnrollmentEntity MapRowToEntity((string, object)[] entityValueTuples)
        {
            return new EnrollmentEntity
            {
                EnrollmentId = GetValueFromTuple<int>("EnrollmentId", entityValueTuples),
                ApprovalStatusId = GetValueFromTuple<byte>("ApprovalStatusId", entityValueTuples),
                ApproverAccountId = GetValueFromTuple<short>("ApproverAccountId", entityValueTuples),
                EmployeeId = GetValueFromTuple<short>("EmployeeId", entityValueTuples),
                RequestedAt = GetValueFromTuple<DateTime>("RequestedAt", entityValueTuples),
                TrainingId = GetValueFromTuple<short>("TrainingId", entityValueTuples),
                UpdatedAt = GetValueFromTuple<DateTime>("UpdatedAt", entityValueTuples)
            };
        }
    }
}
