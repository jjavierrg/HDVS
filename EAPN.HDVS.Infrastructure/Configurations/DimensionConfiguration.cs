using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class DimensionConfiguration : IEntityTypeConfiguration<Dimension>
    {
        public void Configure(EntityTypeBuilder<Dimension> builder)
        {
            builder.ToTable("Dimensiones", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(nameof(Dimension.Id)).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Orden).HasColumnName(nameof(Dimension.Orden)).IsRequired();
            builder.Property(x => x.IconoId).HasColumnName(nameof(Dimension.IconoId));
            builder.Property(x => x.Descripcion).HasColumnName(nameof(Dimension.Descripcion)).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Activo).HasColumnName(nameof(Dimension.Activo)).IsRequired();

            builder.HasOne(x => x.Icono).WithMany().HasForeignKey(x => x.IconoId);
            builder.HasMany(x => x.Categorias).WithOne(x => x.Dimension).HasForeignKey(x => x.DimensionId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
