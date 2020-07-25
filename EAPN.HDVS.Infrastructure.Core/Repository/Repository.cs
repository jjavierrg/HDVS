using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EAPN.HDVS.Infrastructure.Core.Repository
{
    public class Repository<T> : ReadRepository<T>, IRepository<T> where T : class
    {
        public Repository(DbContext context, ILogger<Repository<T>> logger = null) : base(context, logger)
        { }

        public virtual async Task<int> SaveChangesAsync()
        {
            try
            {
                return await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, ex.Message);
                throw;
            }
        }

        public virtual void Add(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            Context.Add(item);
        }

        /// <inheritdoc />
        public virtual void AddRange(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            Context.AddRange(items);
        }

        /// <inheritdoc />
        public virtual void Update(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            Context.Update(item);
        }

        /// <inheritdoc />
        public virtual void UpdateRange(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            Context.UpdateRange(items);
        }

        /// <inheritdoc />
        public virtual void Remove(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            Context.Remove(item);
        }

        public virtual void RemoveRange(Expression<Func<T, bool>> filter)
        {
            var query = GetQuery(filter, null, null);
            Context.RemoveRange(query);
        }

        public virtual void RemoveRange(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            Context.RemoveRange(items);
        }
    }
}
