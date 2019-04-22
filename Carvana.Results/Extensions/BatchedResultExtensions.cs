using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Carvana
{
    [DebuggerNonUserCode]
    public static class BatchedResultExtensions
    {
        public static Result Flatten(this Result result, params Result[] others) => Flatten(new List<Result> { result }.Concat(others));
        public static Result Flatten(this IEnumerable<Result> results)
        {
            List<Result> failed = results.Where(x => !x.Succeeded()).ToList();
            return failed.Any()
                ? Result.Errored(failed.First().Status, $"Errors: {string.Join(", ", failed)}")
                : Result.Success();
        }
        public static async Task<Result> Flatten(this IEnumerable<Task<Result>> tasks) => (await Task.WhenAll(tasks)).Flatten();
        public static async Task<Result> Flatten(this Task<IEnumerable<Result>> asyncResults) => (await asyncResults).Flatten();

        public static Result<IEnumerable<TOutput>> Flatten<TOutput>(this IEnumerable<Result<TOutput>> results)
        {
            List<Result<TOutput>> r = results.ToList();
            List<Result<TOutput>> failed = r.Where(x => !x.Succeeded()).ToList();
            return failed.Any()
                ? Result<IEnumerable<TOutput>>.Errored(failed.First().Status, string.Join(Environment.NewLine, failed.Select(x => x.ErrorMessage)))
                : Result<IEnumerable<TOutput>>.Success(r.Select(x => x.Content));
        }
        public static async Task<Result<IEnumerable<TOutput>>> Flatten<TOutput>(this Task<IEnumerable<Result<TOutput>>> asyncResults)
            => (await asyncResults).Flatten();
        public static async Task<Result<IEnumerable<TOutput>>> Flatten<TOutput>(this IEnumerable<Task<Result<TOutput>>> tasks)
            => (await Task.WhenAll(tasks)).Flatten();


        public static Result<IEnumerable<TResp>> FlattenValues<TReq, TResp>(this IDictionary<TReq, Result<TResp>> batched) => batched.Values.Flatten();
        public static async Task<Result<IEnumerable<TResp>>> FlattenValues<TReq, TResp>(this Task<IDictionary<TReq, Result<TResp>>> batchedTask)
            => (await batchedTask).Values.Flatten();
        public static async Task<Result<IEnumerable<TResp>>> FlattenValues<TReq, TResp>(this Task<Dictionary<TReq, Result<TResp>>> batchedTask)
            => (await batchedTask).Values.Flatten();

        public static Result<IEnumerable<TResp>> FlattenValues<TReq, TResp>(this Result<IDictionary<TReq, TResp>> batchedResult)
            => batchedResult.Failed()
                ? batchedResult.AsTypedError<IEnumerable<TResp>>()
                : batchedResult.Content.Values.ToList();
        public static Result<IEnumerable<TResp>> FlattenValues<TReq, TResp>(this Result<Dictionary<TReq, TResp>> batchedResult)
            => batchedResult.Failed()
                ? batchedResult.AsTypedError<IEnumerable<TResp>>()
                : batchedResult.Content.Values.ToList();

        public static async Task<Result<IEnumerable<TResp>>> FlattenValues<TReq, TResp>(this Task<Result<IDictionary<TReq, TResp>>> flatBatchedTask)
            => (await flatBatchedTask).FlattenValues();
        public static async Task<Result<IEnumerable<TResp>>> FlattenValues<TReq, TResp>(this Task<Result<Dictionary<TReq, TResp>>> flatBatchedTask)
            => (await flatBatchedTask).FlattenValues();
    }
}
