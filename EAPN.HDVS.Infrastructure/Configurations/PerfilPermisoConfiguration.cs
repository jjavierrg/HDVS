using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class PerfilPermisoConfiguration : IEntityTypeConfiguration<PerfilPermiso>
    {
        public void Configure(EntityTypeBuilder<PerfilPermiso> builder)
        {
            builder.ToTable("PerfilesPermisos", "dbo");
            builder.HasKey(x => new { x.PerfilId, x.PermisoId });

            builder.Property(x => x.PerfilId).HasColumnName(nameof(PerfilPermiso.PerfilId)).IsRequired();
            builder.Property(x => x.PermisoId).HasColumnName(nameof(PerfilPermiso.PermisoId)).IsRequired();
        }
    }
}
