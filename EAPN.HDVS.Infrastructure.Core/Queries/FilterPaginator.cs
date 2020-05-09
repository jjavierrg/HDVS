using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EAPN.HDVS.Infrastructure.Core.Queries
{
    public class FilterPaginator<T> : IFilterPaginable<T> where T : class
    {
        /// <inheritDoc />
        public async Task<QueryResult<T>> Execute(IQueryable<T> queryCollection, IQueryData queryData)
        {
            var filter = GetFilters(queryData);
            return await PaginateAsAsync(queryCollection.Where(filter), queryData);
        }

        /// <inheritDoc />
        public Expression<Func<T, bool>> GetFilters(IQueryData queryData)
        {
            var filter = !string.IsNullOrWhiteSpace(queryData.FilterParameters)
            ? ExpressionFilter.ConvertFromString(queryData.FilterParameters).ToList()
            : new List<ExpressionFilter>();

            return ExpressionRetriever.ConstructAndExpressionTree<T>(filter);
        }

        /// <inheritDoc />
        public async Task<QueryResult<T>> PaginateAsAsync(IQueryable<T> queryCollection, IQueryData queryData)
        {
            var orderStatement = GetOrderStatement(queryData);
            var orderAscending = queryData.Order ?? "";
            var page = queryData.PageIndex < 1 ? 1 : queryData.PageIndex;
            var pageSize = queryData.PageSize < 1 ? 1 : queryData.PageSize;

            var totalResult = await queryCollection.CountAsync();
            var dataResult = await queryCollection.OrderBy(orderStatement).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new QueryResult<T>
            {
                Total = totalResult,
                Data = dataResult,
                Ascending = orderAscending.Trim().Equals("asc", StringComparison.OrdinalIgnoreCase),
                OrderBy = queryData.OrderField ?? ""
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="queryIn"></param>
        /// <returns></returns>
        private Expression<Func<T, object>> GetOrderStatement(IQueryData queryIn)
        {
            return ExpressionRetriever.ContructOrderExpression<T>(string.IsNullOrEmpty(queryIn.OrderField)
                ? "Id"
                : queryIn.OrderField);
        }
    }
}
