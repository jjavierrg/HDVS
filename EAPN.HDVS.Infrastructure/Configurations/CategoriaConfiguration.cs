using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("Categorias", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(nameof(Categoria.Id)).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Orden).HasColumnName(nameof(Categoria.Orden)).IsRequired();
            builder.Property(x => x.Descripcion).HasColumnName(nameof(Categoria.Descripcion)).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Obligatorio).HasColumnName(nameof(Categoria.Obligatorio)).IsRequired();
            builder.Property(x => x.Activo).HasColumnName(nameof(Categoria.Activo)).IsRequired();

            builder.HasOne(x => x.Dimension).WithMany(x => x.Categorias).HasForeignKey(x => x.DimensionId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Indicadores).WithOne(x => x.Categoria).HasForeignKey(x => x.CategoriaId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
