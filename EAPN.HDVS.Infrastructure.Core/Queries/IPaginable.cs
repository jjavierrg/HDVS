using System.Linq;
using System.Threading.Tasks;

namespace EAPN.HDVS.Infrastructure.Core.Queries
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPaginable<T> where T : class
    {
        /// <summary>
        /// </summary>
        /// <param name="queryCollection"></param>
        /// <param name="queryData"></param>
        /// <returns></returns>
        Task<QueryResult<T>> PaginateAsAsync(IQueryable<T> queryCollection, IQueryData queryData);

    }
}
