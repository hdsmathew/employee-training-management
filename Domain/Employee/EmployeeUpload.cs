using System;

namespace Core.Domain
{
    public class EmployeeUpload
    {
        public Prerequisite Prerequisite { get; set; }
        public DateTime UploadedAt { get; set; }
        public string UploadPath { get; set; }
    }
}
