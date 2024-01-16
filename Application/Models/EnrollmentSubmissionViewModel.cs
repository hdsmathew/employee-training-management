using System.Collections.Generic;
using System.Web;

namespace Core.Application.Models
{
    public class EnrollmentSubmissionViewModel
    {
        public short TrainingId { get; set; }
        public IEnumerable<HttpPostedFileBase> EmployeeUploads { get; set; }
        public IEnumerable<byte> PrerequisiteIds { get; set; }
    }
}
