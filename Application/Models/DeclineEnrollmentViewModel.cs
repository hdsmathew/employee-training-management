using System.ComponentModel.DataAnnotations;

namespace Core.Application.Models
{
    public class DeclineEnrollmentViewModel
    {
        public int EnrollmentId { get; set; }
        [StringLength(150, ErrorMessage = "Message should be maximum 150 characters long.")]
        public string ReasonMessage { get; set; }
    }
}
