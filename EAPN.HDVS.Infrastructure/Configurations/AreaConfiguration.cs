using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class AreaConfiguration : IEntityTypeConfiguration<Area>
    {
        public void Configure(EntityTypeBuilder<Area> builder)
        {
            builder.ToTable("Areas", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(nameof(Area.Id)).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.IconoId).HasColumnName(nameof(Area.IconoId));
            builder.Property(x => x.Descripcion).HasColumnName(nameof(Area.Descripcion)).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Activo).HasColumnName(nameof(Area.Activo)).IsRequired();

            builder.HasOne(x => x.Icono).WithMany().HasForeignKey(x => x.IconoId);
            builder.HasMany(x => x.Dimensiones).WithOne(x => x.Area).HasForeignKey(x => x.AreaId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
