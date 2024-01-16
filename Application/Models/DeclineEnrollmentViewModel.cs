using System.ComponentModel.DataAnnotations;

namespace Core.Application.Models
{
    public class DeclineEnrollmentViewModel
    {
        public int EnrollmentId { get; set; }
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Message can only contain characters.")]
        [StringLength(150, ErrorMessage = "Message should be maximum 150 characters long.")]
        [Required(ErrorMessage = "Decline Reason is required.")]
        public string ReasonMessage { get; set; }
    }
}
