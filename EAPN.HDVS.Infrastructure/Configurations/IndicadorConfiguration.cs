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
            builder.Property(x => x.DimensionId).HasColumnName(nameof(Indicador.DimensionId)).IsRequired();
            builder.Property(x => x.Descripcion).HasColumnName(nameof(Indicador.Descripcion)).IsRequired();
            builder.Property(x => x.Activo).HasColumnName(nameof(Indicador.Activo)).IsRequired();
            builder.Property(x => x.Puntuacion).HasColumnName(nameof(Indicador.Puntuacion)).IsRequired();
            builder.Property(x => x.Sugerencias).HasColumnName(nameof(Indicador.Sugerencias));

            builder.HasOne(x => x.Dimension).WithMany(x => x.Indicadores).HasForeignKey(x => x.DimensionId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Fichas).WithOne(x => x.Indicador).HasForeignKey(x => x.IndicadorId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
