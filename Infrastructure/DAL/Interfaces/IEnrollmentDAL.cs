using Infrastructure.Models;
using System.Collections.Generic;

namespace Infrastructure.DAL.Interfaces
{
    public interface IEnrollmentDAL
    {
        int Add(EnrollmentModel enrollment);
        int AddWithEmployeeUploads(EnrollmentModel enrollment, IEnumerable<EmployeeUploadModel> employeeUploads);
        int Delete(int enrollmentID);
        bool Exists(short employeeID, short trainingID);
        EnrollmentModel Get(int enrollmentID);
        IEnumerable<EnrollmentModel> GetAll();
        int Update(EnrollmentModel enrollment);
    }
}
