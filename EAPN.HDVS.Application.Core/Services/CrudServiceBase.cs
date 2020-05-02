using EAPN.HDVS.Infrastructure.Core.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EAPN.HDVS.Application.Core.Services
{
    public class CrudServiceBase<T> : ReadServiceBase<T>, ICrudServiceBase<T> where T : class
    {
        private readonly IRepository<T> _repository;
        IRepository<T> ICrudServiceBase<T>.Repository => _repository;

        public CrudServiceBase(IRepository<T> repository, ILogger<CrudServiceBase<T>> logger) : base(repository, logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public T Add(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            _repository.Add(item);
            return item;
        }

        public IEnumerable<T> AddRange(IEnumerable<T> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (!items.Any()) return items;

            _repository.AddRange(items);
            return items;
        }

        public void Remove(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            _repository.Remove(item);
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (!items.Any()) return;

            _repository.RemoveRange(items);
        }

        public void RemoveRange(Expression<Func<T, bool>> filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            _repository.RemoveRange(filter);
        }

        public void Update(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            _repository.Update(item);
        }

        public void UpdateRange(IEnumerable<T> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (!items.Any()) return;

            _repository.UpdateRange(items);
        }

        protected override void ReleaseManagedResources()
        {
            _repository?.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _repository.SaveChangesAsync();
        }
    }
}
