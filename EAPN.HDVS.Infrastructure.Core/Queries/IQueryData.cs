namespace EAPN.HDVS.Infrastructure.Core.Queries
{
    public interface IQueryData
    {
        /// <summary>
        /// String with the custom filters
        /// </summary>
        string FilterParameters { get; }
        /// <summary>
        /// Size of the page
        /// </summary>
        int PageSize { get; }
        /// <summary>
        /// Index of the page
        /// </summary>
        int PageIndex { get; }
        /// <summary>
        /// Field to order by
        /// </summary>
        string OrderField { get; }

        /// <summary>
        /// Order direction
        /// </summary>
        string Order { get; }
    }
}
