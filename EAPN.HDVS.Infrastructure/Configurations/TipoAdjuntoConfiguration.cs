using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class TipoAdjuntoConfiguration : IEntityTypeConfiguration<TipoAdjunto>
    {
        public void Configure(EntityTypeBuilder<TipoAdjunto> builder)
        {
            builder.ToTable("TiposAdjunto", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(nameof(TipoAdjunto.Id)).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Descripcion).HasColumnName(nameof(TipoAdjunto.Descripcion)).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Carpeta).HasColumnName(nameof(TipoAdjunto.Carpeta)).HasMaxLength(255).IsRequired();
        }
    }
}
