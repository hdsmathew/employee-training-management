using Core.Application.Models;
using Core.Domain;

namespace Core.Application.Services
{
    public interface IEmployeeService
    {
        ResponseModel<Employee> Register(Employee employee);
        ResponseModel<Employee> Update(Employee employee);
    }
}
