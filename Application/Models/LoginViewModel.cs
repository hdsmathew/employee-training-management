using System.ComponentModel.DataAnnotations;

namespace Core.Application.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email format.")]
        public string EmailAddress { get; set; }
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
           ErrorMessage = "Password should contain at least 8 characters( including a letter, a digit and an special character).")]
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
