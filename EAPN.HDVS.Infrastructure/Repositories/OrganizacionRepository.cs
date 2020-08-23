using EAPN.HDVS.Entities;
using EAPN.HDVS.Infrastructure.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EAPN.HDVS.Infraestructure.Repositories
{
    public class OrganizacionRepository : Repository<Organizacion>
    {
        public OrganizacionRepository(DbContext context, ILogger<Repository<Organizacion>> logger = null) : base(context, logger)
        {
        }

        public override void Add(Organizacion item)
        {
            item.FechaAlta = DateTime.Now;
            base.Add(item);
        }
        public override void AddRange(IEnumerable<Organizacion> items)
        {
            foreach (var item in items)
            {
                item.FechaAlta = DateTime.Now;
            }

            base.AddRange(items);
        }
    }
}
