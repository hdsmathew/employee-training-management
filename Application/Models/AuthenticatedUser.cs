using Core.Domain;
using System.Linq;

namespace Core.Application.Models
{
    public class AuthenticatedUser
    {
        public AuthenticatedUser(short accountId, AccountTypeEnum accountType)
        {
            AccountId = accountId;
            AccountType = accountType;
        }

        public short AccountId { get; set; }
        public AccountTypeEnum AccountType { get; set; }

        public bool IsInRole(params AccountTypeEnum[] roles)
        {
            return roles.Contains(AccountType);
        }
    }
}
