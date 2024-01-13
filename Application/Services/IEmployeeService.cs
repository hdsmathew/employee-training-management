using Core.Application.Models;
using Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    public interface IEmployeeService
    {
        Task<ResultT<Employee>> GetEmployeeWithUploadsAsync(short employeeId);
        Task<ResultT<IEnumerable<EmployeeUpload>>> GetEmployeeUploadsByEnrollmentIdAsync(int enrollmentId);
        Task<ResultT<IEnumerable<Employee>>> GetManagersAsync();
        Task<Result> RegisterAsync(RegisterViewModel model);
        Task<Result> UpdateAsync(Employee employee);
    }
}
