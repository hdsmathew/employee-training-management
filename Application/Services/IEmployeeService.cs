using Core.Application.Models;
using Core.Domain;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    public interface IEmployeeService
    {
        Task<ResponseModel<Employee>> GetEmployeeUploadsAsync(short employeeId);
        Task<ResponseModel<EmployeeUpload>> GetEmployeeUploadsByEnrollmentIdAsync(int enrollmentId);
        Task<ResponseModel<Employee>> GetManagersAsync();
        Task<ResponseModel<Employee>> RegisterAsync(RegisterViewModel model);
        Task<ResponseModel<Employee>> UpdateAsync(Employee employee);
    }
}
