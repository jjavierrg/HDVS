using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EAPN.HDVS.Infrastructure.Core.Repository
{
    public interface IReadRepository<T> : IDisposable where T : class
    {
        IQueryable<T> EntitySet { get; }
        Task<T> GetSingleOrDefault(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
        Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
        Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
        Task<long> GetCountAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
    }
}
