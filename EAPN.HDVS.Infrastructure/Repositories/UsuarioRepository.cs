using EAPN.HDVS.Entities;
using EAPN.HDVS.Infrastructure.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EAPN.HDVS.Infraestructure.Repositories
{
    public class UsuarioRepository : Repository<Usuario>
    {
        public UsuarioRepository(DbContext context, ILogger<Repository<Usuario>> logger = null) : base(context, logger)
        {
        }

        public override void Add(Usuario item)
        {
            item.FechaAlta = DateTime.Now;
            base.Add(item);
        }
        public override void AddRange(IEnumerable<Usuario> items)
        {
            foreach (var item in items)
            {
                item.FechaAlta = DateTime.Now;
            }

            base.AddRange(items);
        }

        public override void Update(Usuario item)
        {
            base.Update(item);
            Context.Entry(item).Property(x => x.Hash).IsModified = !string.IsNullOrWhiteSpace(item.Hash);
        }

        public override void UpdateRange(IEnumerable<Usuario> items)
        {
            base.UpdateRange(items);
            foreach (var item in items)
            {
                Context.Entry(item).Property(x => x.Hash).IsModified = !string.IsNullOrWhiteSpace(item.Hash);
            }
        }
    }
}
