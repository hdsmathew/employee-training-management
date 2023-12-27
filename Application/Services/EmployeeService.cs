using Core.Application.Models;
using Core.Application.Repositories;
using Core.Domain;

namespace Core.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger _logger;

        public EmployeeService(IEmployeeRepository employeeRepository, ILogger logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
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
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in registering employee");
                response.AddError(new ErrorModel()
                {
                    Message = "Employee registration failed. Try again later.",
                    Exception = dalEx
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
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, $"Error in updating employee");
                response.AddError(new ErrorModel()
                {
                    Message = $"Unable to update employee with Id: {employee.EmployeeId}",
                    Exception = dalEx
                });
            }
            return response;
        }


    }
}
