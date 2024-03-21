using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shared
{
    public interface IValidationResult
    {
        public static readonly Error ValidationError = new(
            "ValidationError",
            "A validation problem occurred.");

        Error[] Errors { get; }
    }

    public sealed class ValidationResult : Result, IValidationResult
    {
        public Error[] Errors { get; }

        public ValidationResult(Error[] errors)
            : base(false, IValidationResult.ValidationError)
        {
            Errors = errors ?? throw new ArgumentNullException(nameof(errors));
        }

        public static ValidationResult WithErrors(Error[] errors) => new(errors);
    }

    public sealed class ValidationResult<TValue> : Result<TValue>, IValidationResult
    {
        public Error[] Errors { get; }

        public ValidationResult(Error[] errors)
            : base(default, false, IValidationResult.ValidationError)
        {
            Errors = errors ?? throw new ArgumentNullException(nameof(errors));
        }

        public static ValidationResult<TValue> WithErrors(Error[] errors) => new(errors);
    }
}
