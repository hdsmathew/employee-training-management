using System.ComponentModel.DataAnnotations;

namespace Core.Application.Models
{
    public class RegisterViewModel
    {
        [Compare("Password", ErrorMessage = "Password and Confirmation Password do not match.")]
        public string ConfirmPassword { get; set; }
        public byte DepartmentId { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [Required(ErrorMessage = "Email is required.")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }
        public short ManagerId { get; set; }
        [RegularExpression(@"^\d{8}$", ErrorMessage = "Invalid mobile number.")]
        [Required(ErrorMessage = "Mobile Number is required.")]
        public string MobileNumber { get; set; }
        [RegularExpression(@"^[A-Z]\d{12}[A-Z]$", ErrorMessage = "Invalid National ID.")]
        [Required(ErrorMessage = "National ID is required.")]
        public string NationalId { get; set; }
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
           ErrorMessage = "Password should contain at least 8 characters( including a letter, a digit and a special character).")]
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
