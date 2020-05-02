using EAPN.HDVS.Infrastructure.Core.Repository;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EAPN.HDVS.Application.Core.Services
{
    public class ReadServiceBase<T> : ServiceBase, IReadServiceBase<T> where T : class
    {
        public IReadRepository<T> Repository { get; }
        protected ILogger<ReadServiceBase<T>> Logger { get; }

        public ReadServiceBase(IReadRepository<T> repository, ILogger<ReadServiceBase<T>> logger)
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            Logger = logger;
        }

        public async Task<T> GetSingleOrDefault(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            return await Repository.GetSingleOrDefault(filter, includes, orderBy);
        }

        public async Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            return await Repository.GetFirstOrDefault(filter, includes, orderBy);
        }

        public async Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            return await Repository.GetListAsync(filter, includes, orderBy);
        }

        public async Task<long> GetCountAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            return await Repository.GetCountAsync(filter, includes, orderBy);
        }

        protected override void ReleaseManagedResources()
        {
            Repository?.Dispose();
        }
    }
}
