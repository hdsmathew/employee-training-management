using Core.Application.Models;
using Core.Domain;
using System.Collections.Generic;

namespace Core.Application.Services
{
    public interface IEnrollmentService
    {
        ResponseModel<Enrollment> Process(Enrollment enrollment);
        ResponseModel<Enrollment> Submit(short employeeId, short trainingId, IEnumerable<EmployeeUpload> employeeUploads);
        ResponseModel<Enrollment> ValidateApprovedEnrollments();
    }
}
