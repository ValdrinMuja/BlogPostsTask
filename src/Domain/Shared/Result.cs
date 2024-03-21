using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shared
{

    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        protected internal Result(TValue? value, bool isSuccess, Error error)
            : base(isSuccess, error) => _value = value;

        public TValue Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("The value of a failure result cannot be accessed!");

        public static implicit operator Result<TValue>(TValue? value) => Create(value);
    }

    public class Result
    {
        protected internal Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None)
            {
                throw new InvalidOperationException("Success result cannot have an error.");
            }

            if (!isSuccess && error == Error.None)
            {
                throw new InvalidOperationException("Failure result must have an error.");
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }

        public bool IsFailure => !IsSuccess;

        public Error Error { get; }

        public static Result Success() => new(true, Error.None);

        public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

        public static Result Failure(Error error) => new(false, error);

        public static Result<TValue> Failure<TValue>(Error error) => new(default(TValue), false, error);

        public static Result<TValue> Create<TValue>(TValue value)
            => value is null ? Failure<TValue>(Error.NullValue) : Success(value);
    }

    public class Error
    {
        public static readonly Error None = new(string.Empty, string.Empty);
        public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null");

        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public string Code { get; }

        public string Message { get; }
    }
}
