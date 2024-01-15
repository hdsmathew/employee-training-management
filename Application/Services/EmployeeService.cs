using Core.Application.Models;
using Core.Application.Repositories;
using Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger _logger;

        public EmployeeService(IEmployeeRepository employeeRepository, ILogger logger, IAccountRepository accountRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
            _accountRepository = accountRepository;
        }

        public async Task<ResultT<Employee>> GetEmployeeWithUploadsAsync(short employeeId)
        {
            try
            {
                Employee employee = await _employeeRepository.GetWithEmployeeUploadsAsync(employeeId);

                return (employee == null)
                    ? ResultT<Employee>.Failure(new Error("Could not retrieve employee with uploads."))
                    : ResultT<Employee>.Success(employee);
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in retrieving employee with uploads.");
            }
            catch (MapperException mapperEx)
            {
                _logger.LogError(mapperEx, "Error in mapper");
            }

            return ResultT<Employee>.Failure(new Error("Unable to retrieve employee with uploads."));
        }

        public async Task<ResultT<IEnumerable<EmployeeUpload>>> GetEmployeeUploadsByEnrollmentIdAsync(int enrollmentId)
        {
            try
            {
                IEnumerable<EmployeeUpload> employeeUploads = await _employeeRepository.GetEmployeeUploadsByEnrollmentIdAsync(enrollmentId);

                return ResultT<IEnumerable<EmployeeUpload>>.Success(employeeUploads);
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, $"Error in retrieving employee uploads with enrollment id: {enrollmentId}.");
            }
            catch (MapperException mapperEx)
            {
                _logger.LogError(mapperEx, "Error in mapper");
            }

            return ResultT<IEnumerable<EmployeeUpload>>.Failure(new Error("Unable to retrieve employee uploads."));
        }

        public async Task<ResultT<IEnumerable<Employee>>> GetManagersAsync()
        {
            try
            {
                IEnumerable<Employee> employees = await _employeeRepository.GetAllByAccountTypeAsync(AccountTypeEnum.Manager);

                return ResultT<IEnumerable<Employee>>.Success(employees);
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in retrieving managers.");
            }
            catch (MapperException mapperEx)
            {
                _logger.LogError(mapperEx, "Error in mapper");
            }
             
            return ResultT<IEnumerable<Employee>>.Failure(new Error("Unable to retrieve managers."));
        }

        public async Task<Result> RegisterAsync(RegisterViewModel model)
        {
            try
            {
                if (await _accountRepository.ExistsByEmailAddress(model.EmailAddress))
                {
                    return Result.Failure(new Error($"Account with EmailAddress: {model.EmailAddress} already exists."));
                }

                if (await _employeeRepository.ExistsByNationalIdOrMobileNumber(model.MobileNumber, model.NationalId))
                {
                    return Result.Failure(new Error($"Employee with NationalId: {model.NationalId} or MobileNumber: {model.MobileNumber} already exists."));
                }

                await _accountRepository.AddWithEmployeeDetails(
                    new Account()
                    {
                        AccountType = AccountTypeEnum.Employee,
                        EmailAddress = model.EmailAddress,
                        PasswordHash = GetPasswordHash(model.Password)
                    },
                    new Employee()
                    {
                        DepartmentId = model.DepartmentId,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        ManagerId = model.ManagerId,
                        MobileNumber = model.MobileNumber,
                        NationalId = model.NationalId,
                    });

                return Result.Success();
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in registering employee");
            }
            catch (MapperException mapperEx)
            {
                _logger.LogError(mapperEx, "Error in mapper");
            }
             
            return ResultT<Employee>.Failure(new Error("Employee registration failed. Try again later."));
        }

        public async Task<Result> UpdateAsync(Employee employee)
        {
            try
            {
                await _employeeRepository.Update(employee);

                return Result.Success();
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, $"Error in updating employee");
            }
            catch (MapperException mapperEx)
            {
                _logger.LogError(mapperEx, "Error in mapper");
            }
             
            return ResultT<Employee>.Failure(new Error($"Unable to update employee with Id: {employee.EmployeeId}"));
        }

        private string GetPasswordHash(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 11);
        }
    }
}
