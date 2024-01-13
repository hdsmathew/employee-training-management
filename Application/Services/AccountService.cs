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

        public async Task<ResultT<AuthenticatedUser>> AuthenticateAsync(LoginViewModel model)
        {
            try
            {
                Account account = await _accountRepository.GetByEmailAddressAsync(model.EmailAddress);
                if (account is null || !IsPasswordValid(model.Password, account.PasswordHash))
                {
                    return ResultT<AuthenticatedUser>.Failure(new Error("Invalid EmailAddress or Password."));
                }

                Employee employee = await _employeeRepository.GetByAccountIdAsync(account.AccountId);
                if (employee is null)
                {
                    return ResultT<AuthenticatedUser>.Failure(new Error("Could not retrieve employee associated with account."));
                }

                return ResultT<AuthenticatedUser>.Success(new AuthenticatedUser(account, employee));
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in authenticating user");
                return ResultT<AuthenticatedUser>.Failure(new Error("Unable to authenticate. Try again later."));
            }
        }

        public async Task<Result> CreateAsync(Account account)
        {
            // TODO: Hash password
            try
            {
                if (await _accountRepository.ExistsByEmailAddress(account.EmailAddress))
                {
                    return Result.Failure(new Error($"Account with EmailAddress: {account.EmailAddress} already exists."));
                }

                await _accountRepository.Add(account);

                return Result.Success();
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in creating account");
                return Result.Failure(new Error("Account registration failed. Try again later."));
            }
        }

        private bool IsPasswordValid(string providedPassword, string savedPasswordHash)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(providedPassword, savedPasswordHash);
        }
    }
}
