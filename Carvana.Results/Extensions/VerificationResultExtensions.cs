using System;
using System.Threading.Tasks;

namespace Carvana
{
    public static class VerificationResultExtensions
    {
        public static Result Verify(this Result input, Func<Result> getVerifiedResult)
        {
            return input.Succeeded()
                ? getVerifiedResult()
                : input;
        }

        public static async Task<Result> Verify(this Result input, Func<Task<Result>> getAsyncVerifiedResult)
        {
            return input.Succeeded()
                ? await getAsyncVerifiedResult()
                : input;
        }

        public static async Task<Result> Verify(this Task<Result> asyncInput, Func<Result> getVerifiedResult)
        {
            Result input = await asyncInput;
            return input.Succeeded()
                ? getVerifiedResult()
                : input;
        }

        public static async Task<Result> Verify(this Task<Result> asyncInput, Func<Task<Result>> getAsyncVerifiedResult)
        {
            Result input = await asyncInput;
            return input.Succeeded()
                ? await getAsyncVerifiedResult()
                : input;
        }

        public static Result<T> Verify<T>(this Result<T> input, Func<T, Result<T>> getVerifiedResult)
        {
            return input.Succeeded()
                ? getVerifiedResult(input.Content)
                : input;
        }

        public static async Task<Result<T>> Verify<T>(this Result<T> input, Func<T, Task<Result<T>>> getAsyncVerifiedResult)
        {
            return input.Succeeded()
                ? await getAsyncVerifiedResult(input.Content)
                : input;
        }

        public static async Task<Result<T>> Verify<T>(this Task<Result<T>> asyncInput, Func<T, Result<T>> getVerifiedResult)
        {
            Result<T> input = await asyncInput;
            return input.Succeeded()
                ? getVerifiedResult(input.Content)
                : input;
        }

        public static async Task<Result<T>> Verify<T>(this Task<Result<T>> asyncInput, Func<T, Task<Result<T>>> getAsyncVerifiedResult)
        {
            Result<T> input = await asyncInput;
            return input.Succeeded()
                ? await getAsyncVerifiedResult(input.Content)
                : input;
        }
    }
}
