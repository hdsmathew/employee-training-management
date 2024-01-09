using Core.Application.Repositories;
using Core.Domain;
using Infrastructure.Common;
using Infrastructure.DAL.Interfaces;
using Infrastructure.Models;
using System.Collections.Generic;

namespace Infrastructure.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly IEnrollmentDAL _enrollmentDAL;
        private readonly MapperBase<EmployeeUpload, EmployeeUploadModel> _employeeUploadMapper;
        private readonly MapperBase<Enrollment, EnrollmentModel> _enrollmentMapper;

        public EnrollmentRepository(IEnrollmentDAL enrollmentDAL, EmployeeUploadMapper employeeUploadMapper, EnrollmentMapper enrollmentMapper)
        {
            _enrollmentDAL = enrollmentDAL;
            _employeeUploadMapper = employeeUploadMapper;
            _enrollmentMapper = enrollmentMapper;
        }

        public int Add(Enrollment enrollment)
        {
            EnrollmentModel enrollmentEntity = _enrollmentMapper.MapEntityToDataModel(enrollment);
            return _enrollmentDAL.Add(enrollmentEntity);
        }

        public int AddWithEmployeeUploads(Enrollment enrollment, IEnumerable<EmployeeUpload> employeeUploads)
        {
            EnrollmentModel enrollmentEntity = _enrollmentMapper.MapEntityToDataModel(enrollment);
            IEnumerable<EmployeeUploadModel> employeeUploadEntities = _employeeUploadMapper.MapEntitiesToDataModels(employeeUploads);
            return _enrollmentDAL.AddWithEmployeeUploads(enrollmentEntity, employeeUploadEntities);
        }

        public int Delete(int enrollmentID)
        {
            return _enrollmentDAL.Delete(enrollmentID);
        }

        public bool Exists(short employeeID, short trainingID)
        {
            return _enrollmentDAL.Exists(employeeID, trainingID);
        }

        public Enrollment Get(int enrollmentID)
        {
            EnrollmentModel enrollmentEntity = _enrollmentDAL.Get(enrollmentID);
            return _enrollmentMapper.MapDataModelToEntity(enrollmentEntity);
        }

        public IEnumerable<Enrollment> GetAll()
        {
            IEnumerable<EnrollmentModel> enrollmentEntities = _enrollmentDAL.GetAll();
            return _enrollmentMapper.MapDataModelsToEntities(enrollmentEntities);
        }

        public IEnumerable<Enrollment> GetAllByTrainingIdAndApprovalStatus(short trainingId, IEnumerable<ApprovalStatusEnum> approvalStatusEnums)
        {
            IEnumerable<EnrollmentModel> enrollmentEntities = _enrollmentDAL.GetAllByTrainingIdAndApprovalStatus(trainingId, approvalStatusEnums);
            return _enrollmentMapper.MapDataModelsToEntities(enrollmentEntities);
        }

        public IEnumerable<Enrollment> GetAllByEmployeeIdAndApprovalStatus(short employeeId, IEnumerable<ApprovalStatusEnum> approvalStatusEnums)
        {
            IEnumerable<EnrollmentModel> enrollmentEntities = _enrollmentDAL.GetAllByEmployeeIdAndApprovalStatus(employeeId, approvalStatusEnums);
            return _enrollmentMapper.MapDataModelsToEntities(enrollmentEntities);
        }

        public IEnumerable<Enrollment> GetAllByManagerIdAndApprovalStatus(short managerId, IEnumerable<ApprovalStatusEnum> approvalStatusEnums)
        {
            IEnumerable<EnrollmentModel> enrollmentEntities = _enrollmentDAL.GetAllByManagerIdAndApprovalStatus(managerId, approvalStatusEnums);
            return _enrollmentMapper.MapDataModelsToEntities(enrollmentEntities);
        }

        public int Update(Enrollment enrollment)
        {
            EnrollmentModel enrollmentEntity = _enrollmentMapper.MapEntityToDataModel(enrollment);
            return _enrollmentDAL.Update(enrollmentEntity);
        }

        public int UpdateBatch(IEnumerable<Enrollment> enrollments)
        {
            IEnumerable<EnrollmentModel> enrollmentEntities = _enrollmentMapper.MapEntitiesToDataModels(enrollments);
            return _enrollmentDAL.UpdateBatch(enrollmentEntities);
        }
    }
}
