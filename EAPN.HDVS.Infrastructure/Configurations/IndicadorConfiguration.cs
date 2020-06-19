using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class IndicadorConfiguration : IEntityTypeConfiguration<Indicador>
    {
        public void Configure(EntityTypeBuilder<Indicador> builder)
        {
            builder.ToTable("Indicadores", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(nameof(Indicador.Id)).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Orden).HasColumnName(nameof(Indicador.Orden)).IsRequired();
            builder.Property(x => x.CategoriaId).HasColumnName(nameof(Indicador.CategoriaId)).IsRequired();
            builder.Property(x => x.Descripcion).HasColumnName(nameof(Indicador.Descripcion)).IsRequired();
            builder.Property(x => x.Activo).HasColumnName(nameof(Indicador.Activo)).IsRequired();
            builder.Property(x => x.Puntuacion).HasColumnName(nameof(Indicador.Puntuacion)).IsRequired();
            builder.Property(x => x.Verificacion).HasColumnName(nameof(Indicador.Verificacion));

            builder.HasOne(x => x.Categoria).WithMany(x => x.Indicadores).HasForeignKey(x => x.CategoriaId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Seguimientos).WithOne(x => x.Indicador).HasForeignKey(x => x.IndicadorId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
