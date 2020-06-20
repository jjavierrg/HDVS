namespace EAPN.HDVS.Application.Services.Dimension
{
    using EAPN.HDVS.Application.Core.Services;
    using EAPN.HDVS.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDimensionService : ICrudServiceBase<Dimension>
    {
        /// <summary>
        /// Get only active dimension data
        /// </summary>
        /// <returns>List of active data related to dimensions</returns>
        Task<IEnumerable<Dimension>> GetActiveDimensionsAsync();
    }
}
