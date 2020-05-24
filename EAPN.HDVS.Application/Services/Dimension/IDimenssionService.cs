namespace EAPN.HDVS.Application.Services.Dimension
{
    using EAPN.HDVS.Application.Core.Services;
    using EAPN.HDVS.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDimenssionService : ICrudServiceBase<Dimension>
    {
        /// <summary>
        /// Get only active dimenssion data
        /// </summary>
        /// <returns>List of active data related to dimenssions</returns>
        Task<IEnumerable<Dimension>> GetActiveDimenssionsAsync();
    }
}
