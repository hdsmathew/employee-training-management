using System;

namespace Core.Application.Models
{
    public class ResultT<T> : Result
    {
        private readonly T _value;

        private ResultT(T value, bool isSuccess, Error error) : base(isSuccess, error)
        {
            _value = value;
        }

        public T Value => IsSuccess 
            ? _value 
            : throw new InvalidOperationException("Cannot access value of a failure result.");

        public static ResultT<T> Success(T value) => new ResultT<T>(value, true, Error.None);
        public static new ResultT<T> Failure(Error error) => new ResultT<T>(default, false, error);
    }
}
