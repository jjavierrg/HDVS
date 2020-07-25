using AutoMapper;
using EAPN.HDVS.Infrastructure.Core.Queries;
using System.Collections.Generic;

namespace EAPN.HDVS.Web.Extensions
{
    public static class MapperExtension
    {
        public static IEnumerable<T> MapList<T>(this IMapper mapper, IEnumerable<object> collection) where T : class, new()
        {
            return mapper.Map<IEnumerable<T>>(collection);
        }

        public static QueryResult<T> MapQueryResult<O, T>(this IMapper mapper, QueryResult<O> queryResult) where T : class, new() where O : class
        {
            if (queryResult == null)
            {
                return null;
            }

            var result = new QueryResult<T>
            {
                Ascending = queryResult.Ascending,
                OrderBy = queryResult.OrderBy,
                Total = queryResult.Total,
                Data = mapper.MapList<T>(queryResult.Data)
            };

            return result;
        }
    }
}
