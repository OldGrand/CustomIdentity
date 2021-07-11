using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomIdentity.BLL.Extensions
{
    public static class LinqExtensions
    {
        public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> source)
        {
            var result = new List<T>();
            await foreach (var item in source)
            {
                result.Add(item);
            }

            return result;
        }
    }
}
