using System;
using System.Linq.Expressions;

namespace EAPN.HDVS.Infrastructure.Core.Queries
{
    public interface IFilterable<T> where T : class
    {
        /// <summary>
        /// Get filters expresion fom query data
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        Expression<Func<T, bool>> GetFilters(IQueryData queryData);
    }
}
