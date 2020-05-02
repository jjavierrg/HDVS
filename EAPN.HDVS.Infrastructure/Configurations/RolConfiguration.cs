using EAPN.HDVS.Entities;
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

            builder.HasData(
                new Rol { Id = 1, Descripcion = "user:login" },
                new Rol { Id = 2, Descripcion = "usermng:read" },
                new Rol { Id = 3, Descripcion = "usermng:write" },
                new Rol { Id = 4, Descripcion = "usermng:delete" }
            );
        }
    }
}
