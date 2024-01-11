﻿using Core.Application.Models;
using Core.Application.Repositories;
using Core.Domain;
using System;
using System.Linq;
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

        public async Task<ResponseModel<Employee>> GetEmployeeUploadsAsync(short employeeId)
        {
            ResponseModel<Employee> response = new ResponseModel<Employee>();
            try
            {
                response.Entity = await _employeeRepository.GetWithEmployeeUploadsAsync(employeeId);
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in retrieving employee with uploads.");
                response.AddError(new ErrorModel()
                {
                    Message = "Unable to retrieve employee with uploads.",
                    Exception = dalEx
                });
            }
            return response;
        }

        public async Task<ResponseModel<EmployeeUpload>> GetEmployeeUploadsByEnrollmentIdAsync(int enrollmentId)
        {
            ResponseModel<EmployeeUpload> response = new ResponseModel<EmployeeUpload>();
            try
            {
                response.Entities = await _employeeRepository.GetEmployeeUploadsByEnrollmentIdAsync(enrollmentId);
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, $"Error in retrieving employee uploads with enrollment id: {enrollmentId}.");
                response.AddError(new ErrorModel()
                {
                    Message = "Unable to retrieve employee uploads.",
                    Exception = dalEx
                });
            }
            return response;
        }

        public async Task<ResponseModel<Employee>> GetManagersAsync()
        {
            ResponseModel<Employee> response = new ResponseModel<Employee>();
            try
            {
                response.Entities = await _employeeRepository.GetAllByAccountTypeAsync(AccountTypeEnum.Manager);
                if (!response.Entities.Any())
                {
                    response.AddError(new ErrorModel()
                    {
                        Message = "No managers found."
                    });
                    return response;
                }
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in retrieving managers.");
                response.AddError(new ErrorModel()
                {
                    Message = "Unable to retrieve managers.",
                    Exception = dalEx
                });
            }
            return response;
        }

        public async Task<ResponseModel<Employee>> RegisterAsync(RegisterViewModel model)
        {
            ResponseModel<Employee> response = new ResponseModel<Employee>();
            try
            {
                if (await _accountRepository.ExistsByEmailAddress(model.EmailAddress))
                {
                    response.AddError(new ErrorModel()
                    {
                        Message = $"Account with EmailAddress: {model.EmailAddress} already exists."
                    });
                    return response;
                }

                if (await _employeeRepository.ExistsByNationalIdOrMobileNumber(model.MobileNumber, model.NationalId))
                {
                    response.AddError(new ErrorModel()
                    {
                        Message = $"Employee with NationalId: {model.NationalId} or MobileNumber: {model.MobileNumber} already exists."
                    });
                    return response;
                }
                response.AddedRows = await _accountRepository.AddWithEmployeeDetails(
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

        public async Task<ResponseModel<Employee>> UpdateAsync(Employee employee)
        {
            ResponseModel<Employee> response = new ResponseModel<Employee>();
            try
            {
                response.UpdatedRows = await _employeeRepository.Update(employee);
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

        private string GetPasswordHash(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 11);
        }
    }
}
