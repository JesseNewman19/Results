using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Carvana
{
    [DebuggerNonUserCode]
    public static class GeneralResultExtensions
    {
        public static Result<TOutput> AsTypedError<TOutput>(this Result result)
        {
            if (result.Succeeded())
                throw new InvalidOperationException($"Successful Result cannot be returned as Typed Error: {result.ErrorMessage}");
            return Result<TOutput>.Errored(result.Status, result.ErrorMessage);
        }

        public static Result<TOutput> AsTypedError<TOutput>(this Result result, string errorMessagePrefix)
        {
            if (result.Succeeded())
                throw new InvalidOperationException($"Successful Result cannot be returned as Typed Error: {result.ErrorMessage}");
            var errorMessage = string.Join("\n", errorMessagePrefix, result.ErrorMessage);
            return Result<TOutput>.Errored(result.Status, errorMessage);
        }
    }
}
