using Core.Application.Models;
using Core.Domain;

namespace Core.Application.Services
{
    public interface IEmployeeService
    {
        ResponseModel<Employee> GetEmployeeUploads(short employeeId);
        ResponseModel<Employee> GetManagers();
        ResponseModel<Employee> Register(RegisterViewModel model);
        ResponseModel<Employee> Update(Employee employee);
    }
}
