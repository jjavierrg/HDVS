using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class IndicadorSeguimientoConfiguration : IEntityTypeConfiguration<IndicadorSeguimiento>
    {
        public void Configure(EntityTypeBuilder<IndicadorSeguimiento> builder)
        {
            builder.ToTable("IndicadoresSeguimientos", "dbo");
            builder.HasKey(x => new { x.SeguimientoId, x.IndicadorId });

            builder.Property(x => x.SeguimientoId).HasColumnName(nameof(IndicadorSeguimiento.SeguimientoId)).IsRequired();
            builder.Property(x => x.IndicadorId).HasColumnName(nameof(IndicadorSeguimiento.IndicadorId)).IsRequired();
            builder.Property(x => x.Observaciones).HasColumnName(nameof(IndicadorSeguimiento.Observaciones));

            builder.HasOne(x => x.Seguimiento).WithMany(x => x.Indicadores).HasForeignKey(x => x.SeguimientoId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Indicador).WithMany(x => x.Seguimientos).HasForeignKey(x => x.IndicadorId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
