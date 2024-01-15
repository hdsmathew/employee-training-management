using Core.Application.Repositories;
using Core.Domain;
using Infrastructure.Common;
using Infrastructure.DAL.Interfaces;
using Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public Task Add(Enrollment enrollment)
        {
            EnrollmentModel enrollmentEntity = _enrollmentMapper.MapEntityToDataModel(enrollment);
            return _enrollmentDAL.AddAsync(enrollmentEntity);
        }

        public Task AddWithEmployeeUploads(Enrollment enrollment, IEnumerable<EmployeeUpload> employeeUploads)
        {
            EnrollmentModel enrollmentEntity = _enrollmentMapper.MapEntityToDataModel(enrollment);
            IEnumerable<EmployeeUploadModel> employeeUploadEntities = _employeeUploadMapper.MapEntitiesToDataModels(employeeUploads);
            return _enrollmentDAL.AddWithEmployeeUploadsAsync(enrollmentEntity, employeeUploadEntities);
        }

        public Task Delete(int enrollmentID)
        {
            return _enrollmentDAL.DeleteAsync(enrollmentID);
        }

        public Task<bool> Exists(short employeeID, short trainingID)
        {
            return _enrollmentDAL.ExistsAsync(employeeID, trainingID);
        }

        public async Task<Enrollment> GetAsync(int enrollmentID)
        {
            EnrollmentModel enrollmentEntity = await _enrollmentDAL.GetAsync(enrollmentID);
            return _enrollmentMapper.MapDataModelToEntity(enrollmentEntity);
        }

        public async Task<IEnumerable<Enrollment>> GetAllAsync()
        {
            IEnumerable<EnrollmentModel> enrollmentEntities = await _enrollmentDAL.GetAllAsync();
            return _enrollmentMapper.MapDataModelsToEntities(enrollmentEntities);
        }

        public async Task<IEnumerable<Enrollment>> GetAllByTrainingIdAndApprovalStatusAsync(short trainingId, IEnumerable<ApprovalStatusEnum> approvalStatusEnums)
        {
            IEnumerable<EnrollmentModel> enrollmentEntities = await _enrollmentDAL.GetAllByTrainingIdAndApprovalStatusAsync(trainingId, approvalStatusEnums);
            return _enrollmentMapper.MapDataModelsToEntities(enrollmentEntities);
        }

        public async Task<IEnumerable<Enrollment>> GetAllByEmployeeIdAndApprovalStatusAsync(short employeeId, IEnumerable<ApprovalStatusEnum> approvalStatusEnums)
        {
            IEnumerable<EnrollmentModel> enrollmentEntities = await _enrollmentDAL.GetAllByEmployeeIdAndApprovalStatusAsync(employeeId, approvalStatusEnums);
            return _enrollmentMapper.MapDataModelsToEntities(enrollmentEntities);
        }

        public async Task<IEnumerable<Enrollment>> GetAllByManagerIdAndApprovalStatusAsync(short managerId, IEnumerable<ApprovalStatusEnum> approvalStatusEnums)
        {
            IEnumerable<EnrollmentModel> enrollmentEntities = await _enrollmentDAL.GetAllByManagerIdAndApprovalStatusAsync(managerId, approvalStatusEnums);
            return _enrollmentMapper.MapDataModelsToEntities(enrollmentEntities);
        }

        public Task Update(Enrollment enrollment)
        {
            EnrollmentModel enrollmentEntity = _enrollmentMapper.MapEntityToDataModel(enrollment);
            return _enrollmentDAL.UpdateAsync(enrollmentEntity);
        }

        public Task UpdateBatch(IEnumerable<Enrollment> enrollments)
        {
            IEnumerable<EnrollmentModel> enrollmentEntities = _enrollmentMapper.MapEntitiesToDataModels(enrollments);
            return _enrollmentDAL.UpdateBatchAsync(enrollmentEntities);
        }
    }
}
