using EAPN.HDVS.Infrastructure.Core.Queries;
using System.Collections.Generic;

namespace EAPN.HDVS.Web.Dto
{
    /// <summary>
    /// </summary>
    public class GraphQuery
    {
        /// <exclude />
        public QueryData Query { get; set; }
        /// <exclude />
        public IEnumerable<int> Rangos { get; set; }
        /// <exclude />
        public bool GlobalData { get; set; }
    }
}
