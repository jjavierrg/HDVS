using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class RangoConfiguration : IEntityTypeConfiguration<Rango>
    {
        public void Configure(EntityTypeBuilder<Rango> builder)
        {
            builder.ToTable("Rangos", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(nameof(Rango.Id)).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Descripcion).HasColumnName(nameof(Rango.Descripcion)).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Minimo).HasColumnName(nameof(Rango.Minimo)).IsRequired();
            builder.Property(x => x.Maximo).HasColumnName(nameof(Rango.Maximo));
        }
    }
}
