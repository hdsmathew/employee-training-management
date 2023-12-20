using System;

namespace Core.Application.Models
{
    public class ErrorModel
    {
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }
}
