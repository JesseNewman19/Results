using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Carvana
{
    [DebuggerNonUserCode]
    public static class SuccessResultExtensions
    {
        public static Result OnSuccess(this Result result, Action onSuccess)
        {
            if (result.Succeeded())
                onSuccess();
            return result;
        }

        public static Result<T> OnSuccess<T>(this Result<T> result, Action<T> onSuccess)
        {
            if (result.Succeeded())
                onSuccess(result.Content);
            return result;
        }
        
        /* Do NOT make an extension method with this signature. It causes ambiguity with tasks. */
        // public static async Task<Result> OnSuccess(this Task<Result> asyncResult, Action onSuccess)
        
        /* Do NOT make an extension method with this signature. It causes ambiguity with tasks. */
        // public static async Task<Result> OnSuccess(this Task<Result> asyncResult, Action<Result> onSuccess)

        /* Do NOT make an extension method with this signature. It causes ambiguity with tasks. */ 
        // public static async Task<Result<T>> OnSuccess<T>(this Task<Result<T>> asyncResult, Action onSuccess)

        /* Do NOT make an extension method with this signature. It causes massive ambiguity with tasks. */
        // public static async Task<Result<T>> OnSuccess<T>(this Task<Result<T>> asyncResult, Action<T> onSuccess)
        
        /* Do NOT make an extension method with this signature. It causes ambiguity with tasks. */
        // public static async Task<Result<T>> OnSuccess<T>(this Task<Result<T>> asyncResult, Action<Result> onSuccess)
        
        /* Do NOT make an extension method with this signature. It causes ambiguity with tasks. */
        // public static async Task<Result<T>> OnSuccess<T>(this Task<Result<T>> asyncResult, Action<Result<T>> onSuccess)

        public static async Task<Result<T>> OnSuccess<T>(this Result<T> result, Func<Task> onSuccessAsync)
        {
            if (result.Succeeded())
                await onSuccessAsync();
            return result;
        }

        public static async Task<Result<T>> OnSuccess<T>(this Result<T> result, Func<T, Task> onSuccessAsync)
        {
            if (result.Succeeded())
                await onSuccessAsync(result.Content);
            return result;
        }
        
        public static async Task<Result> OnSuccess(this Result result, Func<Task<Result>> onSuccessAsync)
        {
            if (result.Failed())
                return result;
            
            Result onSuccessResult = await onSuccessAsync();
            return onSuccessResult.Succeeded()
                ? result
                : onSuccessResult;
        }

        public static async Task<Result> OnSuccess(this Task<Result> asyncResult, Func<Task<Result>> onSuccessAsync)
        {
            Result result = await asyncResult;
            if (result.Failed())
                return result;
            
            Result onSuccessResult = await onSuccessAsync();
            return onSuccessResult.Succeeded()
                ? result
                : onSuccessResult;
        }
        
        public static async Task<Result<T>> OnSuccess<T>(this Task<Result<T>> asyncResult, Func<T, Task<Result>> onSuccessAsync)
        {
            Result<T> result = await asyncResult;
            if (result.Failed())
                return result;
            
            Result onSuccessResult = await onSuccessAsync(result.Content);
            return onSuccessResult.Succeeded()
                ? result
                : onSuccessResult.AsTypedError<T>();
        }

        public static async Task<Result<T>> OnSuccess<T>(this Task<Result<T>> asyncResult, Func<T, Task> onSuccessAsync)
        {
            Result<T> result = await asyncResult;
            if (result.Succeeded())
                await onSuccessAsync(result.Content);
            return result;
        }

        public static Result<TOutput> Then<TOutput>(this Result input, Func<TOutput> getOutput)
        {
            return input.Succeeded()
                ? getOutput()
                : Result<TOutput>.Errored(input.Status, input.ErrorMessage);
        }

        public static Result<TOutput> Then<TOutput>(this Result input, Func<Result<TOutput>> getOutputResult)
        {
            return input.Succeeded()
                ? getOutputResult()
                : Result<TOutput>.Errored(input.Status, input.ErrorMessage);
        }

        public static async Task<Result<TOutput>> Then<TOutput>(this Result input, Func<Task<Result<TOutput>>> getAsyncOutputResult)
        {
            return input.Succeeded()
                ? await getAsyncOutputResult()
                : Result<TOutput>.Errored(input.Status, input.ErrorMessage);
        }

        public static async Task<Result<TOutput>> Then<TOutput>(this Task<Result> asyncInput, Func<TOutput> getOutput)
        {
            Result input = await asyncInput;
            return input.Succeeded()
                ? getOutput()
                : Result<TOutput>.Errored(input.Status, input.ErrorMessage);
        }

        public static async Task<Result<TOutput>> Then<TOutput>(this Task<Result> asyncInput, Func<Task<TOutput>> getOutputAsync)
        {
            Result input = await asyncInput;
            return input.Succeeded()
                ? await getOutputAsync()
                : Result<TOutput>.Errored(input.Status, input.ErrorMessage);
        }

        public static async Task<Result<TOutput>> Then<TOutput>(this Task<Result> asyncInput, Func<Result<TOutput>> getOutputResult)
        {
            var input = await asyncInput;
            return input.Succeeded()
                ? getOutputResult()
                : Result<TOutput>.Errored(input.Status, input.ErrorMessage);
        }

        public static async Task<Result<TOutput>> Then<TOutput>(this Task<Result> asyncInput, Func<Task<Result<TOutput>>> getAsyncOutputResult)
        {
            var input = await asyncInput;
            return input.Succeeded()
                ? await getAsyncOutputResult()
                : Result<TOutput>.Errored(input.Status, input.ErrorMessage);
        }

        public static Result<TOutput> Then<TInput, TOutput>(this Result<TInput> input, Func<TInput, TOutput> getOutput)
        {
            return input.Succeeded()
                ? getOutput(input.Content)
                : Result<TOutput>.Errored(input.Status, input.ErrorMessage);
        }

        public static Result<TOutput> Then<TInput, TOutput>(this Result<TInput> input, Func<TInput, Result<TOutput>> getOutputResult)
        {
            return input.Succeeded()
                ? getOutputResult(input.Content)
                : Result<TOutput>.Errored(input.Status, input.ErrorMessage);
        }

        public static async Task<Result<TOutput>> Then<TInput, TOutput>(this Result<TInput> input,
            Func<TInput, Task<Result<TOutput>>> getAsyncOutputResult)
        {
            return input.Succeeded()
                ? await getAsyncOutputResult(input.Content)
                : Result<TOutput>.Errored(input.Status, input.ErrorMessage);
        }

        public static async Task<Result<TOutput>> Then<TInput, TOutput>(this Task<Result<TInput>> asyncInput, Func<TInput, TOutput> getOutput)
        {
            Result<TInput> input = await asyncInput;
            return input.Succeeded()
                ? Result.Success(getOutput(input.Content))
                : Result<TOutput>.Errored(input.Status, input.ErrorMessage);
        }

        public static async Task<Result<TOutput>> Then<TInput, TOutput>(this Task<Result<TInput>> asyncInput, Func<TInput, Task<TOutput>> getOutputAsync)
        {
            Result<TInput> input = await asyncInput;
            return input.Succeeded()
                ? Result.Success(await getOutputAsync(input.Content))
                : Result<TOutput>.Errored(input.Status, input.ErrorMessage);
        }

        public static async Task<Result<TOutput>> Then<TInput, TOutput>(this Task<Result<TInput>> asyncInput,
            Func<TInput, Result<TOutput>> getOutputResult)
        {
            Result<TInput> input = await asyncInput;
            return input.Succeeded()
                ? getOutputResult(input.Content)
                : Result<TOutput>.Errored(input.Status, input.ErrorMessage);
        }

        public static async Task<Result<TOutput>> Then<TInput, TOutput>(this Task<Result<TInput>> asyncInput,
            Func<TInput, Task<Result<TOutput>>> getAsyncOutputResult)
        {
            Result<TInput> input = await asyncInput;
            return input.Succeeded()
                ? await getAsyncOutputResult(input.Content)
                : Result<TOutput>.Errored(input.Status, input.ErrorMessage);
        }

        public static async Task<Result<(T1, T2)>> ThenConcatWith<T1, T2>(this Task<Result<T1>> firstResultTask,
            Func<T1, Task<Result<T2>>> concatFunc)
        {
            Result<T1> firstResult = await firstResultTask;
            if (firstResult.Failed())
                return new Result<(T1, T2)>(firstResult.Status, firstResult.ErrorMessage);

            Result<T2> secondResult = await concatFunc(firstResult.Content);
            if (secondResult.Failed())
                return new Result<(T1, T2)>(secondResult.Status, secondResult.ErrorMessage);

            return (firstResult.Content, secondResult.Content);
        }

        public static async Task<Result<(T1, T2, T3)>> ThenConcatWith<T1, T2, T3>(this Task<Result<(T1, T2)>> previousConcatTask,
            Func<(T1, T2), Task<Result<T3>>> nextConcatFunc)
        {
            Result<(T1, T2)> previousConcatResult = await previousConcatTask;
            if (previousConcatResult.Failed())
                return new Result<(T1, T2, T3)>(previousConcatResult.Status, previousConcatResult.ErrorMessage);

            Result<T3> thirdResult = await nextConcatFunc(previousConcatResult.Content);
            if (thirdResult.Failed())
                return new Result<(T1, T2, T3)>(thirdResult.Status, thirdResult.ErrorMessage);

            return (previousConcatResult.Content.Item1, previousConcatResult.Content.Item2, thirdResult.Content);
        }

        public static async Task<Result<(T1, T2, T3, T4)>> ThenConcatWith<T1, T2, T3, T4>(this Task<Result<(T1, T2, T3)>> previousConcatTask,
            Func<(T1, T2, T3), Task<Result<T4>>> nextConcatFunc)
        {
            Result<(T1, T2, T3)> previousConcatResult = await previousConcatTask;
            if (previousConcatResult.Failed())
                return new Result<(T1, T2, T3, T4)>(previousConcatResult.Status, previousConcatResult.ErrorMessage);

            Result<T4> fourthResult = await nextConcatFunc(previousConcatResult.Content);
            if (fourthResult.Failed())
                return new Result<(T1, T2, T3, T4)>(fourthResult.Status, fourthResult.ErrorMessage);

            return (previousConcatResult.Content.Item1, previousConcatResult.Content.Item2, previousConcatResult.Content.Item3, fourthResult.Content);
        }
    }
}
