using System;

namespace Infrastructure.Entities
{
    public class EmployeeUploadEntity : EntityBase
    {
        public ushort EmployeeUploadId { get; set; }
        public byte Prerequisite {  get; set; }
        public DateTime UploadedAt { get; set; }
        public string UploadPath { get; set; }
    }
}
