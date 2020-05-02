using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class PerfilRolConfiguration : IEntityTypeConfiguration<PerfilRol>
    {
        public void Configure(EntityTypeBuilder<PerfilRol> builder)
        {
            builder.ToTable("PerfilesRoles", "dbo");
            builder.HasKey(x => new { x.PerfilId, x.RolId });

            builder.Property(x => x.PerfilId).HasColumnName(nameof(PerfilRol.PerfilId)).IsRequired();
            builder.Property(x => x.RolId).HasColumnName(nameof(PerfilRol.RolId)).IsRequired();

            builder.HasData(
                new PerfilRol { PerfilId = 1, RolId = 1 },
                new PerfilRol { PerfilId = 2, RolId = 1 },
                new PerfilRol { PerfilId = 2, RolId = 2 },
                new PerfilRol { PerfilId = 2, RolId = 3 },
                new PerfilRol { PerfilId = 2, RolId = 4 }
            );
        }
    }
}
