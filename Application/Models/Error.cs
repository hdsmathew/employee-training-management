namespace Core.Application.Models
{
    public class Error
    {
        public static readonly Error None = new Error(string.Empty);

        public Error(string message) { Message = message; }
        public string Message { get; }

        public override string ToString()
        {
            return Message;
        }
    }
}
