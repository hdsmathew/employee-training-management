using System;

namespace Infrastructure.Entities
{
    public class EmployeeUploadEntity : IEntity
    {
        public ushort EmployeeUploadId { get; set; }
        public byte Prerequisite {  get; set; }
        public DateTime UploadedAt { get; set; }
        public string UploadPath { get; set; }
    }
}
