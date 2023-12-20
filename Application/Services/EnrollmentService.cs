using Core.Application.Models;
using Core.Application.Repositories;
using Core.Domain;
using System;

namespace Core.Application.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;

        public EnrollmentService(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public ResponseModel<Enrollment> Process(Enrollment enrollment)
        {
            ResponseModel<Enrollment> response = new ResponseModel<Enrollment>();
            try
            {
                response.UpdatedRows = _enrollmentRepository.Update(enrollment);
            }
            catch (Exception ex)
            {
                response.AddError(new ErrorModel()
                {
                    Message = "Unable to process enrollment. Try again later.",
                    Exception = ex
                });
            }
            return response;
        }

        public ResponseModel<Enrollment> Submit(Enrollment enrollment)
        {
            ResponseModel<Enrollment> response = new ResponseModel<Enrollment>();
            try
            {
                if (_enrollmentRepository.Exists(enrollment.EmployeeId, enrollment.TrainingId))
                {
                    response.AddError(new ErrorModel()
                    {
                        Message = $"User already has a pending enrollment submission."
                    });
                    return response;
                }
                response.AddedRows = _enrollmentRepository.Add(enrollment);
            }
            catch (Exception ex)
            {
                response.AddError(new ErrorModel()
                {
                    Message = "Training registration failed. Try again later.",
                    Exception = ex
                });
            }
            return response;
        }

        public ResponseModel<Enrollment> ValidateApprovedEnrollments()
        {
            throw new System.NotImplementedException();
        }
    }
}
