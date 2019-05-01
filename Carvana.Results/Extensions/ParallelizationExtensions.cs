using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Carvana
{
    [DebuggerNonUserCode]
    public static class ParallelizationExtensions
    {
        public static async Task<Result> Parallelized(this IEnumerable<Task<Result>> tasks) 
            => (await Task.WhenAll(tasks)).Flatten();

        public static async Task<Result<IEnumerable<TOutput>>> Parallelized<TOutput>(this IEnumerable<Task<Result<TOutput>>> tasks) 
            => (await Task.WhenAll(tasks)).Flatten();
    }
}
