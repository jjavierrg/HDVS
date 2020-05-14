using EAPN.HDVS.Entities;
using EAPN.HDVS.Shared.Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class PermisoConfiguration : IEntityTypeConfiguration<Permiso>
    {
        public void Configure(EntityTypeBuilder<Permiso> builder)
        {
            builder.ToTable("Permisos", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(nameof(Permiso.Id)).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Descripcion).HasColumnName(nameof(Permiso.Descripcion)).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Clave).HasColumnName(nameof(Permiso.Clave)).HasMaxLength(50).IsRequired();
        }
    }
}
