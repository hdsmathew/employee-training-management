using System;

namespace Core.Application.Models
{
    public class Error
    {
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }
}
