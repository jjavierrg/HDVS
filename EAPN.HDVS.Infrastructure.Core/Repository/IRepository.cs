using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EAPN.HDVS.Infrastructure.Core.Repository
{
    public interface IRepository<T> : IReadRepository<T> where T : class
    {
        Task<int> SaveChangesAsync();
        void Add(T item);
        void AddRange(IEnumerable<T> items);
        void Update(T item);
        void UpdateRange(IEnumerable<T> items);
        void Remove(T item);
        void RemoveRange(IEnumerable<T> items);
        void RemoveRange(Expression<Func<T, bool>> filter);
    }
}
