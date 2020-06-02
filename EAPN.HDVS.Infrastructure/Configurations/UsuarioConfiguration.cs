using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(nameof(Usuario.Id)).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.OrganizacionId).HasColumnName(nameof(Usuario.OrganizacionId)).IsRequired();
            builder.Property(x => x.Nombre).HasColumnName(nameof(Usuario.Nombre)).HasMaxLength(150);
            builder.Property(x => x.Apellidos).HasColumnName(nameof(Usuario.Apellidos)).HasMaxLength(150);
            builder.Property(x => x.Email).HasColumnName(nameof(Usuario.Email)).IsRequired().HasMaxLength(250);
            builder.Property(x => x.Telefono).HasColumnName(nameof(Usuario.Telefono)).HasMaxLength(20);
            builder.Property(x => x.Hash).HasColumnName(nameof(Usuario.Hash)).IsRequired().HasMaxLength(128);
            builder.Property(x => x.IntentosLogin).HasColumnName(nameof(Usuario.IntentosLogin)).IsRequired();
            builder.Property(x => x.Activo).HasColumnName(nameof(Usuario.Activo)).IsRequired();
            builder.Property(x => x.UltimoAcceso).HasColumnName(nameof(Usuario.UltimoAcceso));
            builder.Property(x => x.FinBloqueo).HasColumnName(nameof(Usuario.FinBloqueo));
            builder.Property(x => x.Observaciones).HasColumnName(nameof(Usuario.Observaciones));
            builder.Property(x => x.FechaAlta).HasColumnName(nameof(Usuario.FechaAlta)).IsRequired();

            builder.Ignore(x => x.NombreCompleto);

            builder.HasMany(x => x.Perfiles).WithOne(x => x.Usuario).HasForeignKey(x => x.UsuarioId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.PermisosAdicionales).WithOne(x => x.Usuario).HasForeignKey(x => x.UsuarioId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Tokens).WithOne(x => x.Usuario).HasForeignKey(x => x.UsuarioId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Organizacion).WithMany().HasForeignKey(x => x.OrganizacionId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
