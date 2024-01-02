using System;

namespace Core.Domain
{
    public class EmployeeUpload : IEntity
    {
        public EmployeeUpload() { }
        public EmployeeUpload(short employeeId, byte prerequisiteId, string uploadPath)
        {
            EmployeeId = employeeId;
            PrerequisiteId = prerequisiteId;
            UploadedAt = DateTime.UtcNow;
            UploadPath = uploadPath;
        }

        public short EmployeeId { get; set; }
        public byte PrerequisiteId { get; set; }
        public DateTime UploadedAt { get; set; }
        public string UploadPath { get; set; }
    }
}
