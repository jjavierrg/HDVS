﻿namespace EAPN.HDVS.Application.Services.Dimension
{
    using EAPN.HDVS.Application.Core.Services;
    using EAPN.HDVS.Entities;
    using EAPN.HDVS.Infrastructure.Core.Repository;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    internal class DimensionService : CrudServiceBase<Dimension>, IDimensionService
    {
        public DimensionService(IRepository<Dimension> repository, ILogger<DimensionService> logger) : base(repository, logger)
        {
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Dimension>> GetActiveDimensionsAsync()
        {
            var dimensiones = await GetListAsync(x => x.Activo, q => q.Include(x => x.Categorias).ThenInclude(x => x.Indicadores), q => q.OrderBy(x => x.Orden));

            foreach (var dimension in dimensiones)
            {
                dimension.Categorias = dimension.Categorias.Where(x => x.Activo).OrderBy(x => x.Orden).ToList();
                foreach (var categoria in dimension.Categorias)
                {
                    categoria.Indicadores = categoria.Indicadores.Where(x => x.Activo).OrderBy(x => x.Orden).ToList();
                }
            }

            return dimensiones;
        }
    }
}
