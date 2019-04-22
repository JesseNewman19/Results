using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Carvana
{
    [DebuggerNonUserCode]
    public static class ExceptionResultExtensions
    {
        public static TResult ThrowOnFailure<TResult>(this TResult result)
            where TResult : Result
        {
            if (result.Failed())
                throw new Exception($"Error: {result.ErrorMessage}");
            return result;
        }

        public static async Task<TResult> ThrowOnFailure<TResult>(this Task<TResult> resultTask)
            where TResult : Result
        {
            TResult result = await resultTask;
            if (result.Failed())
                throw new Exception($"Error: {result.ErrorMessage}");
            return result;
        }

        public static TOutput ContentOrThrow<TOutput>(this Result<TOutput> result)
        {
            if (result.Failed())
                throw new Exception($"Error: {result.ErrorMessage}");
            return result.Content;
        }

        public static async Task<TOutput> ContentOrThrow<TOutput>(this Task<Result<TOutput>> resultTask)
        {
            Result<TOutput> result = await resultTask;
            if (result.Failed())
                throw new Exception($"Error: {result.ErrorMessage}");
            return result.Content;
        }

        public static Result<TOutput> ThenGuarded<TInput, TOutput>(this Result<TInput> input, Func<TInput, TOutput> getOutput)
        {
            if (input.Failed())
                return Result<TOutput>.Errored(input.Status, input.ErrorMessage);

            try
            {
                return Result.Success(getOutput(input.Content));
            }
            catch (Exception ex)
            {
                return Result<TOutput>.Errored(ResultStatus.ProcessingError, ex.Message);
            }
        }

        public static async Task<Result<TOutput>> ThenGuarded<TInput, TOutput>(this Task<Result<TInput>> asyncInput, Func<TInput, TOutput> getOutput)
        {
            Result<TInput> input = await asyncInput;
            if (input.Failed())
                return Result<TOutput>.Errored(input.Status, input.ErrorMessage);

            try
            {
                return Result.Success(getOutput(input.Content));
            }
            catch (Exception ex)
            {
                return Result<TOutput>.Errored(ResultStatus.ProcessingError, ex.Message);
            }
        }

        public static async Task<Result<TOutput>> ThenGuarded<TInput, TOutput>(this Task<Result<TInput>> asyncInput, Func<TInput, Task<TOutput>> getOutputAsync)
        {
            Result<TInput> input = await asyncInput;
            if (input.Failed())
                return Result<TOutput>.Errored(input.Status, input.ErrorMessage);

            try
            {
                return Result.Success(await getOutputAsync(input.Content));
            }
            catch (Exception ex)
            {
                return Result<TOutput>.Errored(ResultStatus.ProcessingError, ex.Message);
            }
        }
    }
}
