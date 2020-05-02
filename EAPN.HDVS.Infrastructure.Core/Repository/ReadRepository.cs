using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EAPN.HDVS.Infrastructure.Core.Repository
{
    public class ReadRepository<T> : IReadRepository<T> where T : class
    {
        protected DbContext Context { get; }
        protected readonly ILogger<ReadRepository<T>> Logger;

        public IQueryable<T> EntitySet => Context.Set<T>();

        public ReadRepository(DbContext context, ILogger<ReadRepository<T>> logger = null)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Logger = logger;
        }

        /// <inheritdoc />
        public virtual async Task<T> GetFirstOrDefault(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            var query = GetQuery(filter, includes, orderBy);
            return await query.FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public virtual async Task<T> GetSingleOrDefault(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            var query = GetQuery(filter, includes, orderBy);
            return await query.SingleOrDefaultAsync();
        }

        /// <inheritdoc />

        public virtual async Task<IEnumerable<T>> GetListAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            var query = GetQuery(filter, includes, orderBy);
            return await query.ToListAsync();
        }

        /// <inheritdoc />
        public virtual async Task<long> GetCountAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            var query = GetQuery(filter, includes, orderBy);
            return await query.LongCountAsync();
        }

        protected IQueryable<T> GetQuery(Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            IQueryable<T> query = EntitySet;

            if (includes != null)
                query = includes(query);

            if (orderBy != null)
                query = orderBy(query);

            if (filter != null)
                query = query.Where(filter);

            return query;
        }

        protected virtual void ReleaseManagedResources()
        {
            Context?.Dispose();
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                ReleaseManagedResources();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
