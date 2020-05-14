using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class PaisConfiguration : IEntityTypeConfiguration<Pais>
    {
        public void Configure(EntityTypeBuilder<Pais> builder)
        {
            builder.ToTable("Paises", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(nameof(Pais.Id)).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Descripcion).HasColumnName(nameof(Pais.Descripcion)).HasMaxLength(50).IsRequired();
        }
    }
}
