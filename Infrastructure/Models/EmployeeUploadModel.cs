using System;

namespace Infrastructure.Models
{
    public class EmployeeUploadModel : IModel
    {
        public short EmployeeId { get; set; }
        public byte PrerequisiteId {  get; set; }
        public DateTime UploadedAt { get; set; }
        public string UploadPath { get; set; }
    }
}
