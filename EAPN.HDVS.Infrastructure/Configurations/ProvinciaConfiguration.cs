using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class ProvinciaConfiguration : IEntityTypeConfiguration<Provincia>
    {
        public void Configure(EntityTypeBuilder<Provincia> builder)
        {
            builder.ToTable("Provincias", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(nameof(Provincia.Id)).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Nombre).HasColumnName(nameof(Provincia.Nombre)).HasMaxLength(40).IsRequired();

            builder.HasMany(x => x.Municipios).WithOne(x => x.Provincia).HasForeignKey(x => x.ProvinciaId);
        }
    }
}
