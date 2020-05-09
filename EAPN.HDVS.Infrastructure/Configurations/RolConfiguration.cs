using EAPN.HDVS.Entities;
using EAPN.HDVS.Shared.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class RolConfiguration : IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> builder)
        {
            builder.ToTable("Roles", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(nameof(Rol.Id)).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Descripcion).HasColumnName(nameof(Rol.Descripcion)).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Permiso).HasColumnName(nameof(Rol.Permiso)).HasMaxLength(50).IsRequired();

            builder.HasData(
                new Rol { Id = 1, Descripcion = "Aplicación: Acceder", Permiso = Roles.APP_USERLOGIN },
                new Rol { Id = 2, Descripcion = "Usuarios: Lectura", Permiso = Roles.USERMANAGEMENT_READ},
                new Rol { Id = 3, Descripcion = "Usuarios: Escritura", Permiso = Roles.USERMANAGEMENT_WRITE },
                new Rol { Id = 4, Descripcion = "Usuarios: Eliminar", Permiso = Roles.USERMANAGEMENT_DELETE },
                new Rol { Id = 5, Descripcion = "Usuarios: Acceder", Permiso = Roles.USERMANAGEMENT_ACCESS },
                new Rol { Id = 6, Descripcion = "Usuarios: Superadministrador", Permiso = Roles.USERMANAGEMENT_ADMIN }
            );
        }
    }
}
