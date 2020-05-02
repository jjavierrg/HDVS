using AutoMapper;
using System.Collections.Generic;

namespace EAPN.HDVS.Web.Extensions
{
    public static class MapperExtension
    {
        public static IEnumerable<T> MapList<T>(this IMapper mapper, IEnumerable<object> collection) where T : class, new()
        {
            return mapper.Map<IEnumerable<T>>(collection);
        }
    }
}
