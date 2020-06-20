﻿using EAPN.HDVS.Entities;
using EAPN.HDVS.Infrastructure.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

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
            item.FechaUltimaModificacion = DateTime.Now;
            base.Add(item);
        }
        public override void AddRange(IEnumerable<Ficha> items)
        {
            foreach (var item in items)
            {
                item.FechaAlta = DateTime.Now;
                item.FechaUltimaModificacion = DateTime.Now;
            }

            base.AddRange(items);
        }

        public override void Update(Ficha item)
        {
            item.FechaUltimaModificacion = DateTime.Now;
            base.Update(item);
        }

        public override void UpdateRange(IEnumerable<Ficha> items)
        {
            foreach(var item in items)
                item.FechaUltimaModificacion = DateTime.Now;

            base.UpdateRange(items);
        }
    }
}
