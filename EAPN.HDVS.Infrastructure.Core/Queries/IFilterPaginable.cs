using System.Linq;
using System.Threading.Tasks;

namespace EAPN.HDVS.Infrastructure.Core.Queries
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFilterPaginable<T> : IPaginable<T>, IFilterable<T> where T : class
    {
        /// <summary>
        /// Get result query after filter and pagination
        /// </summary>
        /// <param name="queryCollection"></param>
        /// <param name="queryData"></param>
        /// <returns></returns>
        Task<QueryResult<T>> Execute(IQueryable<T> queryCollection, IQueryData queryData);
    }
}
