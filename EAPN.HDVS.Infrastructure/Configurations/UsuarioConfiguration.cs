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
            builder.Property(x => x.Nombre).HasColumnName(nameof(Usuario.Nombre)).HasMaxLength(150);
            builder.Property(x => x.Apellidos).HasColumnName(nameof(Usuario.Apellidos)).HasMaxLength(150);
            builder.Property(x => x.Email).HasColumnName(nameof(Usuario.Email)).IsRequired().HasMaxLength(250);
            builder.Property(x => x.Hash).HasColumnName(nameof(Usuario.Hash)).IsRequired().HasMaxLength(128);
            builder.Property(x => x.IntentosLogin).HasColumnName(nameof(Usuario.IntentosLogin)).IsRequired();
            builder.Property(x => x.Activo).HasColumnName(nameof(Usuario.Activo)).IsRequired();
            builder.Property(x => x.FinBloqueo).HasColumnName(nameof(Usuario.FinBloqueo));
            builder.Property(x => x.Observaciones).HasColumnName(nameof(Usuario.Observaciones));

            builder.HasMany(x => x.Perfiles).WithOne(x => x.Usuario).HasForeignKey(x => x.UsuarioId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.RolesAdicionales).WithOne(x => x.Usuario).HasForeignKey(x => x.UsuarioId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Tokens).WithOne(x => x.Usuario).HasForeignKey(x => x.UsuarioId).OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                new Usuario { Id = 1, Nombre="José Javier", Apellidos="Rodríguez Gallego", Email = "Jjavierrg@gmail.com", Hash = "$2b$12$sn1hzFuoYJwKxocoCsn3me4gLtEeMG9sJrz/FQQu6XMww3bdoSgOe", IntentosLogin = 0, Activo = true },
                new Usuario { Id = 2, Nombre = "Test", Apellidos = "Test", Email = "test@test.com", Hash = "$2b$12$vrDFckbZnoXbB9oFeLqqn.UQtnQ2UdYOJC/r6UqrjLfS00LagnO0q", IntentosLogin = 0, Activo = true }
            );
        }
    }
}
