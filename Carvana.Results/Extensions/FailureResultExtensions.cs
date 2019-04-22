using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Carvana
{
    [DebuggerNonUserCode]
    public static class FailureResultExtensions
    {
        public static Result OnFailure(this Result result, Action onFailure)
        {
            if (result.Failed())
                onFailure();
            return result;
        }

        public static Result OnFailure(this Result result, Action<Result> onFailure)
        {
            if (result.Failed())
                onFailure(result);
            return result;
        }

        public static Result<T> OnFailure<T>(this Result<T> result, Action<Result<T>> onFailure)
        {
            if (result.Failed())
                onFailure(result);
            return result;
        }

        public static async Task<Result<T>> OnFailure<T>(this Result<T> result, Func<Task> onFailureAsync)
        {
            if (result.Failed())
                await onFailureAsync();
            return result;
        }

        public static async Task<Result> OnFailure(this Result result, Func<Result, Task> onFailureAsync)
        {
            if (result.Failed())
                await onFailureAsync(result);
            return result;
        }

        public static async Task<Result<T>> OnFailure<T>(this Result<T> result, Func<Result<T>, Task> onFailureAsync)
        {
            if (result.Failed())
                await onFailureAsync(result);
            return result;
        }

        public static async Task<Result> OnFailure(this Task<Result> asyncResult, Action onFailure)
        {
            Result result = await asyncResult;
            if (result.Failed())
                onFailure();
            return result;
        }

        public static async Task<Result> OnFailure(this Task<Result> resultTask, Action<Result> onFailure)
        {
            Result result = await resultTask;
            if (result.Failed())
                onFailure(result);
            return result;
        }

        public static async Task<Result> OnFailure(this Task<Result> resultTask, Func<Result, Task> onFailureAsync)
        {
            Result result = await resultTask;
            if (result.Failed())
                await onFailureAsync(result);
            return result;
        }

        public static async Task<Result<T>> OnFailure<T>(this Task<Result<T>> asyncResult, Action<Result<T>> onFailure)
        {
            Result<T> result = await asyncResult;
            if (result.Failed())
                onFailure(result);
            return result;
        }

        public static async Task<Result<T>> OnFailure<T>(this Task<Result<T>> asyncResult, Func<Result<T>, Task> onFailureAsync)
        {
            Result<T> result = await asyncResult;
            if (result.Failed())
                await onFailureAsync(result);
            return result;
        }

        public static Result IfFailedThen(this Result input, Func<Result> getOutputResult)
        {
            return input.Failed()
                ? getOutputResult()
                : input;
        }

        public static async Task<Result> IfFailedThen(this Result input, Func<Task<Result>> getAsyncOutputResult)
        {
            return input.Failed()
                ? await getAsyncOutputResult()
                : input;
        }

        public static async Task<Result> IfFailedThen<TOutput>(this Task<Result> asyncInput, Func<Result> getOutputResult)
        {
            Result input = await asyncInput;
            return input.Failed()
                ? getOutputResult()
                : input;
        }

        public static async Task<Result> IfFailedThen(this Task<Result> asyncInput, Func<Task<Result>> getAsyncOutputResult)
        {
            Result input = await asyncInput;
            return input.Failed()
                ? await getAsyncOutputResult()
                : input;
        }

        public static Result<T> IfFailedThen<T>(this Result<T> input, Func<Result<T>, Result<T>> getOutputResult)
        {
            return input.Failed()
                ? getOutputResult(input)
                : input;
        }

        public static async Task<Result<T>> IfFailedThen<T>(this Result<T> input, Func<Result<T>, Task<Result<T>>> getAsyncOutputResult)
        {
            return input.Failed()
                ? await getAsyncOutputResult(input)
                : input;
        }

        public static async Task<Result<T>> IfFailedThen<T>(this Task<Result<T>> asyncInput, Func<Result<T>, Result<T>> getOutputResult)
        {
            Result<T> input = await asyncInput;
            return input.Failed()
                ? getOutputResult(input)
                : input;
        }

        public static async Task<Result<T>> IfFailedThen<T>(this Task<Result<T>> asyncInput, Func<Result<T>, Task<Result<T>>> getAsyncOutputResult)
        {
            Result<T> input = await asyncInput;
            return input.Failed()
                ? await getAsyncOutputResult(input)
                : input;
        }
        
        public static Result<T> IfFailedWithErrorMessage<T>(this Result<T> input, string message)
        {
            return input.Failed()
                ? new Result<T>(input.Status, message)
                : input;
        }

        public static Result<T> IfFailedWithErrorMessage<T>(this Result<T> input, Func<Result<T>, string> getMessage)
        {
            return input.Failed()
                ? new Result<T>(input.Status, getMessage(input))
                : input;
        }

        public static async Task<Result<T>> IfFailedWithErrorMessage<T>(this Result<T> input, Func<Result<T>, Task<string>> getMessageAsync)
        {
            return input.Failed()
                ? new Result<T>(input.Status, await getMessageAsync(input))
                : input;
        }

        public static async Task<Result<T>> IfFailedWithErrorMessage<T>(this Task<Result<T>> asyncInput, Func<Result<T>, string> getMessage)
        {
            Result<T> input = await asyncInput;
            return input.Failed()
                ? new Result<T>(input.Status, getMessage(input))
                : input;
        }

        public static async Task<Result<T>> IfFailedWithErrorMessage<T>(this Task<Result<T>> asyncInput, Func<Result<T>, Task<string>> getMessageAsync)
        {
            Result<T> input = await asyncInput;
            return input.Failed()
                ? new Result<T>(input.Status, await getMessageAsync(input))
                : input;
        }

        public static Result<T> IfFailedPrependMessage<T>(this Result<T> input, string prefix)
        {
            return input.Failed()
                ? input.AsTypedError<T>(prefix)
                : input;
        }

        public static Result<T> IfFailedPrependMessage<T>(this Result<T> input, Func<Result<T>, string> getPrefix)
        {
            return input.Failed()
                ? input.AsTypedError<T>(getPrefix(input))
                : input;
        }

        public static async Task<Result<T>> IfFailedPrependMessage<T>(this Result<T> input, Func<Result<T>, Task<string>> getPrefixAsync)
        {
            return input.Failed()
                ? input.AsTypedError<T>(await getPrefixAsync(input))
                : input;
        }

        public static async Task<Result<T>> IfFailedPrependMessage<T>(this Task<Result<T>> asyncInput, Func<Result<T>, string> getPrefix)
        {
            Result<T> input = await asyncInput;
            return input.Failed()
                ? input.AsTypedError<T>(getPrefix(input))
                : input;
        }

        public static async Task<Result<T>> IfFailedPrependMessage<T>(this Task<Result<T>> asyncInput, Func<Result<T>, Task<string>> getPrefixAsync)
        {
            Result<T> input = await asyncInput;
            return input.Failed()
                ? input.AsTypedError<T>(await getPrefixAsync(input))
                : input;
        }
    }
}
