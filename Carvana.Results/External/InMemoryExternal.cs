using System;
using System.Threading.Tasks;

namespace Carvana
{
    public sealed class InMemoryExternal<TId, T> : IExternal<TId, T>
    {
        private readonly Func<TId, Task<T>> _getValue;

        public InMemoryExternal(Func<TId, T> getValue) : this(async x => await Task.FromResult(getValue(x))) { }
        public InMemoryExternal(Func<TId, Task<T>> getValue) => _getValue = getValue;

        public async Task<Result<T>> Get(TId key) => await _getValue(key);
    }
}
