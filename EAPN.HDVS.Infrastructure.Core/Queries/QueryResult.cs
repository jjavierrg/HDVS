using System.Collections.Generic;

namespace EAPN.HDVS.Infrastructure.Core.Queries
{
    public class QueryResult<T>
    {
        public int Total { get; set; }
        public string OrderBy { get; set; }
        public bool Ascending { get; set; }
        public IEnumerable<T> Data { get; set; }

        public QueryResult()
        {
            Total = 0;
            OrderBy = string.Empty;
            Ascending = true;
            Data = new List<T>();
        }

        public void InitializeCommonInfo<U>(QueryResult<U> data)
        {
            Total = data.Total;
            OrderBy = data.OrderBy;
            Ascending = data.Ascending;
        }

    }
}
