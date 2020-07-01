using EAPN.HDVS.Application.Core.Services;
using EAPN.HDVS.Entities;
using EAPN.HDVS.Infrastructure.Core.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAPN.HDVS.Application.Services.Seguimientos
{
    public class SeguimientoService : CrudServiceBase<Seguimiento>
    {
        private readonly IRepository<Categoria> _categoriaRepository;
        public SeguimientoService(IRepository<Seguimiento> repository, IRepository<Categoria> categoriaRepository, ILogger<CrudServiceBase<Seguimiento>> logger) : base(repository, logger)
        {
            _categoriaRepository = categoriaRepository ?? throw new ArgumentNullException(nameof(categoriaRepository));
        }

        public override void Update(Seguimiento item)
        {
            item.Completo = IsCompletedAsync(item).Result;
            base.Update(item);
        }

        public override Seguimiento Add(Seguimiento item)
        {
            item.Completo = IsCompletedAsync(item).Result;
            return base.Add(item);
        }

        private async Task<bool> IsCompletedAsync(Seguimiento item)
        {
            if (item.ForzadoIncompleto.HasValue && item.ForzadoIncompleto.Value)
                return false;

            if (item.Completo)
                return true;

            var indicadoresIds = item.Indicadores.Select(x => x.IndicadorId);
            var categoriasObligatorias = await _categoriaRepository.GetListAsync(x => x.Activo && x.Dimension.Activo && x.Obligatorio);
            var categoriasSeguimientos = await _categoriaRepository.GetListAsync(x => x.Indicadores.Any(i => indicadoresIds.Contains(i.Id)));

            return categoriasObligatorias.All(c => categoriasSeguimientos.Contains(c));
        }
    }
}
