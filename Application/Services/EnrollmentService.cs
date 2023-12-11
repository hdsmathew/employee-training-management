using Core.Application.Models;
using Core.Application.Repositories;
using Core.Domain.Enrollment;
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

        public Response<Enrollment> Process(Enrollment enrollment)
        {
            Response<Enrollment> response = new Response<Enrollment>();
            try
            {
                response.UpdatedRows = _enrollmentRepository.Update(enrollment);
            }
            catch (Exception ex)
            {
                response.AddError(new Error()
                {
                    Message = "Unable to process enrollment. Try again later.",
                    Exception = ex
                });
            }
            return response;
        }

        public Response<Enrollment> Submit(Enrollment enrollment)
        {
            Response<Enrollment> response = new Response<Enrollment>();
            try
            {
                if (_enrollmentRepository.Exists(enrollment.EmployeeID, enrollment.TrainingID))
                {
                    response.AddError(new Error()
                    {
                        Message = $"User already has a pending enrollment submission."
                    });
                    return response;
                }
                response.AddedRows = _enrollmentRepository.Add(enrollment);
            }
            catch (Exception ex)
            {
                response.AddError(new Error()
                {
                    Message = "Training registration failed. Try again later.",
                    Exception = ex
                });
            }
            return response;
        }

        public Response<Enrollment> ValidateApprovedEnrollments()
        {
            throw new System.NotImplementedException();
        }
    }
}
