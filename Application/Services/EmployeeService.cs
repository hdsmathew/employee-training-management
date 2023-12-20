using Core.Application.Models;
using Core.Application.Repositories;
using Core.Domain;
using System;

namespace Core.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public ResponseModel<Employee> Register(Employee employee)
        {
            ResponseModel<Employee> response = new ResponseModel<Employee>();
            try
            {
                if (_employeeRepository.ExistsByNationalIdOrMobileNumber(employee.NationalId, employee.MobileNumber))
                {
                    response.AddError(new ErrorModel()
                    {
                        Message = $"Employee with NationalId: {employee.NationalId} or MobileNumber: {employee.MobileNumber} already exists."
                    });
                    return response;
                }
                response.AddedRows = _employeeRepository.Add(employee);
            }
            catch (Exception ex)
            {
                response.AddError(new ErrorModel()
                {
                    Message = "Employee registration failed. Try again later.",
                    Exception = ex
                });
            }
            return response;
        }

        public ResponseModel<Employee> Update(Employee employee)
        {
            ResponseModel<Employee> response = new ResponseModel<Employee>();
            try
            {
                response.UpdatedRows = _employeeRepository.Update(employee);
            }
            catch (Exception ex)
            {
                response.AddError(new ErrorModel()
                {
                    Message = $"Unable to update employee with Id: {employee.EmployeeId}",
                    Exception = ex
                });
            }
            return response;
        }
    }
}
