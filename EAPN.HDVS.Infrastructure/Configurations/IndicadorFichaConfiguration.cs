using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class IndicadorFichaConfiguration : IEntityTypeConfiguration<IndicadorFicha>
    {
        public void Configure(EntityTypeBuilder<IndicadorFicha> builder)
        {
            builder.ToTable("IndicadorFichas", "dbo");
            builder.HasKey(x => new { x.FichaId, x.IndicadorId });

            builder.Property(x => x.FichaId).HasColumnName(nameof(IndicadorFicha.FichaId)).IsRequired();
            builder.Property(x => x.IndicadorId).HasColumnName(nameof(IndicadorFicha.IndicadorId)).IsRequired();
            builder.Property(x => x.Observaciones).HasColumnName(nameof(IndicadorFicha.Observaciones));

            builder.HasOne(x => x.Ficha).WithMany(x => x.Indicadores).HasForeignKey(x => x.FichaId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Indicador).WithMany(x => x.Fichas).HasForeignKey(x => x.IndicadorId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
