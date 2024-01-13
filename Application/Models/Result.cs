using System;

namespace Core.Application.Models
{
    public class Result
    {
        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None ||
                !isSuccess && error == Error.None)
            {
                throw new ArgumentException("Invalid error.");
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        public Error Error { get; }
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        public static Result Success() => new Result(true, Error.None);
        public static Result Failure(Error error) => new Result(false, error);
    }
}
