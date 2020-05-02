using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class UsuarioRolConfiguration : IEntityTypeConfiguration<UsuarioRol>
    {
        public void Configure(EntityTypeBuilder<UsuarioRol> builder)
        {
            builder.ToTable("UsuariosRoles", "dbo");
            builder.HasKey(x => new { x.UsuarioId, x.RolId });

            builder.Property(x => x.UsuarioId).HasColumnName(nameof(UsuarioRol.UsuarioId)).IsRequired();
            builder.Property(x => x.RolId).HasColumnName(nameof(UsuarioRol.RolId)).IsRequired();
        }
    }
}
