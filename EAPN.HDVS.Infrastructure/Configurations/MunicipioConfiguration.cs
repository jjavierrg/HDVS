using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class MunicipioConfiguration : IEntityTypeConfiguration<Municipio>
    {
        public void Configure(EntityTypeBuilder<Municipio> builder)
        {
            builder.ToTable("Municipios", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(nameof(Municipio.Id)).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.ProvinciaId).HasColumnName(nameof(Municipio.ProvinciaId)).IsRequired();
            builder.Property(x => x.Nombre).HasColumnName(nameof(Municipio.Nombre)).HasMaxLength(100).IsRequired();

            builder.HasOne(x => x.Provincia).WithMany(x => x.Municipios).HasForeignKey(x => x.ProvinciaId);
        }
    }
}
