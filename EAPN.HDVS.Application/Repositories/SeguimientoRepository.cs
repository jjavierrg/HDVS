using EAPN.HDVS.Entities;
using EAPN.HDVS.Infrastructure.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

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
        }
        public override void AddRange(IEnumerable<Seguimiento> items)
        {
            foreach (var item in items)
            {
                item.FechaAlta = DateTime.Now;
                item.FechaUltimaModificacion = DateTime.Now;
            }

            base.AddRange(items);
        }

        public override void Update(Seguimiento item)
        {
            item.FechaUltimaModificacion = DateTime.Now;
            base.Update(item);
        }

        public override void UpdateRange(IEnumerable<Seguimiento> items)
        {
            foreach(var item in items)
                item.FechaUltimaModificacion = DateTime.Now;

            base.UpdateRange(items);
        }
    }
}
