using EAPN.HDVS.Entities;
using EAPN.HDVS.Infrastructure.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EAPN.HDVS.Application.Repositories
{
    public class SeguimientoRepository : Repository<Seguimiento>
    {
        public SeguimientoRepository(DbContext context, ILogger<Repository<Seguimiento>> logger = null) : base(context, logger)
        {
        }

        public override void Add(Seguimiento item)
        {
            item.FechaAlta = DateTime.Now;
            item.FechaUltimaModificacion = DateTime.Now;

            base.Add(item);
            UpdateFichas(new int[] { item.FichaId });
        }
        public override void AddRange(IEnumerable<Seguimiento> items)
        {
            foreach (var item in items)
            {
                item.FechaAlta = DateTime.Now;
                item.FechaUltimaModificacion = DateTime.Now;
            }

            base.AddRange(items);
            UpdateFichas(items.Select(x => x.FichaId));
        }

        public override void Update(Seguimiento item)
        {
            item.FechaUltimaModificacion = DateTime.Now;
            base.Update(item);
            UpdateFichas(new int[] { item.FichaId });
        }

        public override void UpdateRange(IEnumerable<Seguimiento> items)
        {
            foreach (var item in items)
            {
                item.FechaUltimaModificacion = DateTime.Now;
            }

            base.UpdateRange(items);
            UpdateFichas(items.Select(x => x.FichaId));
        }

        public override void Remove(Seguimiento item)
        {
            var fichaId = item.FichaId;
            base.Remove(item);
            UpdateFichas(new int[] { fichaId }, new int[] { item.Id });
        }

        private void UpdateFichas(IEnumerable<int> fichasIds, IEnumerable<int> seguimientosExcludedIds = null)
        {
            seguimientosExcludedIds ??= new int[] { };
            var fichas = Context.Set<Ficha>().Where(x => fichasIds.Contains(x.Id)).Include(x => x.Seguimientos).ToList();

            fichas.ForEach(x =>
            {
                var seguimiento = x.Seguimientos.OrderByDescending(x => x.Fecha).FirstOrDefault(x => !seguimientosExcludedIds.Contains(x.Id));
                x.FechaUltimaModificacion = DateTime.Now;
                x.Completa = x.DatosCompletos && seguimiento != null && seguimiento.Completo;
            });
            Context.UpdateRange(fichas);
        }
    }
}
