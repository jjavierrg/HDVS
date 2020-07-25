using EAPN.HDVS.Entities;
using EAPN.HDVS.Infrastructure.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EAPN.HDVS.Application.Repositories
{
    public class FichaRepository : Repository<Ficha>
    {
        public FichaRepository(DbContext context, ILogger<Repository<Ficha>> logger = null) : base(context, logger)
        {
        }

        public override void Add(Ficha item)
        {
            item.FechaAlta = DateTime.Now;
            SetFichaState(item);
            base.Add(item);
        }
        public override void AddRange(IEnumerable<Ficha> items)
        {
            foreach (var item in items)
            {
                item.FechaAlta = DateTime.Now;
                SetFichaState(item);
            }

            base.AddRange(items);
        }

        public override void Update(Ficha item)
        {
            SetFichaState(item);
            base.Update(item);
        }

        public override void UpdateRange(IEnumerable<Ficha> items)
        {
            foreach (var item in items)
            {
                SetFichaState(item);
            }

            base.UpdateRange(items);
        }


        private void SetFichaState(Ficha ficha)
        {
            var seguimiento = Context.Set<Seguimiento>().OrderByDescending(x => x.Fecha).FirstOrDefault(x => x.FichaId == ficha.Id);
            ficha.FechaUltimaModificacion = DateTime.Now;
            ficha.Completa = ficha.DatosCompletos && seguimiento != null && seguimiento.Completo;

            Context.Update(ficha);
        }
    }
}
