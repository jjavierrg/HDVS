using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class UsuarioPerfilConfiguration : IEntityTypeConfiguration<UsuarioPerfil>
    {
        public void Configure(EntityTypeBuilder<UsuarioPerfil> builder)
        {
            builder.ToTable("UsuariosPerfiles", "dbo");
            builder.HasKey(x => new { x.UsuarioId, x.PerfilId });

            builder.Property(x => x.UsuarioId).HasColumnName(nameof(UsuarioPerfil.UsuarioId)).IsRequired();
            builder.Property(x => x.PerfilId).HasColumnName(nameof(UsuarioPerfil.PerfilId)).IsRequired();
        }
    }
}
