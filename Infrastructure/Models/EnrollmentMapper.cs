using Core.Domain;
using Infrastructure.Common;
using System;
using System.Reflection;

namespace Infrastructure.Models
{
    public class EnrollmentMapper : MapperBase<Enrollment, EnrollmentModel>
    {
        public override EnrollmentModel MapEntityToDataModel(Enrollment entity)
        {
            if (entity is null) return null;

            return new EnrollmentModel
            {
                EnrollmentId = entity.EnrollmentId,
                ApprovalStatusId = (byte)entity.ApprovalStatus,
                ApproverAccountId = entity.ApproverAccountId,
                EmployeeId = entity.EmployeeId,
                RequestedAt = entity.RequestedAt,
                TrainingId = entity.TrainingId,
                UpdatedAt = entity.UpdatedAt
            };
        }

        public override Enrollment MapDataModelToEntity(EnrollmentModel model)
        {
            if (model is null) return null;

            return new Enrollment
            {
                EnrollmentId = model.EnrollmentId,
                ApprovalStatus = (ApprovalStatusEnum)model.ApprovalStatusId,
                ApproverAccountId = model.ApproverAccountId,
                EmployeeId = model.EmployeeId,
                RequestedAt = model.RequestedAt,
                TrainingId = model.TrainingId,
                UpdatedAt = model.UpdatedAt
            };
        }

        public override EnrollmentModel MapRowToDataModel((string, object)[] entityValueTuples)
        {
            if (entityValueTuples is null || entityValueTuples.Length == 0) return null;

            return new EnrollmentModel
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
