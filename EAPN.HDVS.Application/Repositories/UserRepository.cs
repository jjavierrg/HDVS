using EAPN.HDVS.Entities;
using EAPN.HDVS.Infrastructure.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace EAPN.HDVS.Application.Repositories
{
    public class UsuarioRepository : Repository<Usuario>
    {
        public UsuarioRepository(DbContext context, ILogger<Repository<Usuario>> logger = null) : base(context, logger)
        {
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
                Context.Entry(item).Property(x => x.Hash).IsModified = !string.IsNullOrWhiteSpace(item.Hash);
        }
    }
}
