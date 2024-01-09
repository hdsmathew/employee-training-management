﻿using Core.Application.Models;
using Core.Application.Repositories;
using Core.Domain;
using System;

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

        public ResponseModel<AuthenticatedUser> Authenticate(LoginViewModel model)
        {
            // TODO: Hash password
            ResponseModel<AuthenticatedUser> response = new ResponseModel<AuthenticatedUser>();
            try
            {
                Account account = _accountRepository.Get(model.EmailAddress, model.Password);
                if (account == null)
                {
                    response.AddError(new ErrorModel()
                    {
                        Message = "Invalid EmailAddress or Password",
                    });
                    return response;
                }
                Employee employee = _employeeRepository.GetByAccountId(account.AccountId);
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

        public ResponseModel<Account> Create(Account account)
        {
            // TODO: Hash password
            ResponseModel<Account> response = new ResponseModel<Account>();
            try
            {
                if (_accountRepository.ExistsByEmailAddress(account.EmailAddress))
                {
                    response.AddError(new ErrorModel()
                    {
                        Message = $"Account with EmailAddress: {account.EmailAddress} already exists."
                    });
                    return response;
                }
                response.AddedRows = _accountRepository.Add(account);
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
    }
}
