using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class UsuarioPermisoConfiguration : IEntityTypeConfiguration<UsuarioPermiso>
    {
        public void Configure(EntityTypeBuilder<UsuarioPermiso> builder)
        {
            builder.ToTable("UsuariosPermisos", "dbo");
            builder.HasKey(x => new { x.UsuarioId, x.PermisoId });

            builder.Property(x => x.UsuarioId).HasColumnName(nameof(UsuarioPermiso.UsuarioId)).IsRequired();
            builder.Property(x => x.PermisoId).HasColumnName(nameof(UsuarioPermiso.PermisoId)).IsRequired();
        }
    }
}
