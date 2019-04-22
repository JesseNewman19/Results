using System.Threading.Tasks;

namespace Carvana
{
    public interface IExternal<in TKey, T>
    {
        Task<Result<T>> Get(TKey key);
    }
}
