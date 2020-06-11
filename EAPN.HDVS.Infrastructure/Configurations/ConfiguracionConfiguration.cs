using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class ConfiguracionConfiguration : IEntityTypeConfiguration<Configuracion>
    {
        public void Configure(EntityTypeBuilder<Configuracion> builder)
        {
            builder.ToTable("Configuraciones", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(nameof(Configuracion.Id)).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.MostrarEnlaces).HasColumnName(nameof(Configuracion.MostrarEnlaces)).IsRequired();
            builder.Property(x => x.Enlaces).HasColumnName(nameof(Configuracion.Enlaces)).HasMaxLength(1000);
        }
    }
}
