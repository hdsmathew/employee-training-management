using Core.Domain;
using System.Linq;

namespace Core.Application.Models
{
    public class AuthenticatedUser
    {
        public AuthenticatedUser(Account account, Employee employee)
        {
            AccountId = account.AccountId;
            AccountType = account.AccountType;
            EmployeeId = employee.EmployeeId;
            FirstName = employee.FirstName;
        }

        public short AccountId { get; set; }
        public AccountTypeEnum AccountType { get; set; }
        public short EmployeeId { get; set; }
        public string FirstName { get; set; }

        public bool IsInRole(params AccountTypeEnum[] roles)
        {
            return roles.Contains(AccountType);
        }
    }
}
