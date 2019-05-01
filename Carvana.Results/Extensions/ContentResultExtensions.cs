using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Carvana
{
    [DebuggerNonUserCode]
    public static class ContentResultExtensions
    {
        public static bool IsMissing<TResult>(this TResult result)
            where TResult : Result
        {
            return result.Status.Equals(ResultStatus.MissingResource);
        }
        
        public static async Task<Result> WithoutContent<T>(this Task<Result<T>> task) => await WithNoContent(task);
        public static async Task<Result> WithNoContent<T>(this Task<Result<T>> task)
        {
            var result = await task;
            return WithNoContent(result);
        } 
        
        public static Result WithoutContent<TResult>(this Result<TResult> result) => WithNoContent(result);
        public static Result WithNoContent<TResult>(this Result<TResult> result)
        {
            return result.Succeeded()
                ? Result.Success()
                : Result.Errored(result.Status, result.ErrorMessage);
        }
        
        public static T OrDefault<T>(this Result<T> input, T defaultValue)
        {
            return input.Failed()
                ? defaultValue
                : input.Content;
        }

        public static T OrDefault<T>(this Result<T> input, Func<T> getDefault)
        {
            return input.Failed()
                ? getDefault()
                : input.Content;
        }

        public static async Task<T> OrDefault<T>(this Task<Result<T>> inputTask, Func<T> getDefaultTask)
        {
            Result<T> input = await inputTask;
            return input.Failed()
                ? getDefaultTask()
                : input.Content;
        }

        public static async Task<T> OrDefault<T>(this Result<T> input, Func<Task<T>> getDefaultTask)
        {
            return input.Failed()
                ? await getDefaultTask()
                : input.Content;
        }

        public static async Task<T> OrDefault<T>(this Task<Result<T>> inputTask, Func<Task<T>> getDefaultTask)
        {
            Result<T> input = await inputTask;
            return input.Failed()
                ? await getDefaultTask()
                : input.Content;
        }
    }
}
