namespace EAPN.HDVS.Infrastructure.Core.Queries
{
    public class QueryData : IQueryData
    {
        public string FilterParameters { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public string OrderField { get; set; }

        public string Order { get; set; }
    }
}
