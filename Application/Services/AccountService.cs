﻿using BCrypt.Net;
using Core.Application.Models;
using Core.Application.Repositories;
using Core.Domain;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger _logger;

        public AccountService(IAccountRepository accountRepository, ILogger logger, IEmployeeRepository employeeRepository)
        {
            _accountRepository = accountRepository;
            _logger = logger;
            _employeeRepository = employeeRepository;
        }

        public async Task<ResponseModel<AuthenticatedUser>> AuthenticateAsync(LoginViewModel model)
        {
            ResponseModel<AuthenticatedUser> response = new ResponseModel<AuthenticatedUser>();
            try
            {
                Account account = await _accountRepository.GetByEmailAddressAsync(model.EmailAddress);
                if (account is null || !IsPasswordValid(model.Password, account.PasswordHash))
                {
                    response.AddError(new ErrorModel()
                    {
                        Message = "Invalid EmailAddress or Password",
                    });
                    return response;
                }

                Employee employee = await _employeeRepository.GetByAccountIdAsync(account.AccountId);
                response.Entity = new AuthenticatedUser(account, employee);
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in authenticating user");
                response.AddError(new ErrorModel()
                {
                    Message = "Unable to authenticate. Try again later.",
                    Exception = dalEx
                });
            }
            return response;
        }

        public async Task<ResponseModel<Account>> CreateAsync(Account account)
        {
            // TODO: Hash password
            ResponseModel<Account> response = new ResponseModel<Account>();
            try
            {
                if (await _accountRepository.ExistsByEmailAddress(account.EmailAddress))
                {
                    response.AddError(new ErrorModel()
                    {
                        Message = $"Account with EmailAddress: {account.EmailAddress} already exists."
                    });
                    return response;
                }
                response.AddedRows = await _accountRepository.Add(account);
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in creating account");
                response.AddError(new ErrorModel()
                {
                    Message = "Account registration failed. Try again later.",
                    Exception = dalEx
                });
            }
            return response;
        }

        private bool IsPasswordValid(string providedPassword, string savedPasswordHash)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(providedPassword, savedPasswordHash);
        }
    }
}
