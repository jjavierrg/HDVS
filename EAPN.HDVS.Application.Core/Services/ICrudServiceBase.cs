using EAPN.HDVS.Infrastructure.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EAPN.HDVS.Application.Core.Services
{
    public interface ICrudServiceBase<T> : IReadServiceBase<T> where T : class
    {
        new IRepository<T> Repository { get; }
        Task<int> SaveChangesAsync();

        T Add(T item);
        IEnumerable<T> AddRange(IEnumerable<T> items);
        void Update(T item);
        void UpdateRange(IEnumerable<T> items);
        void Remove(T item);
        void RemoveRange(IEnumerable<T> items);
        void RemoveRange(Expression<Func<T, bool>> filter);
    }
}
