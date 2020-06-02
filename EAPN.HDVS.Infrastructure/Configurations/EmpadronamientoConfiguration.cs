using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class EmpadronamientoConfiguration : IEntityTypeConfiguration<Empadronamiento>
    {
        public void Configure(EntityTypeBuilder<Empadronamiento> builder)
        {
            builder.ToTable("Empadronamientos", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(nameof(Empadronamiento.Id)).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Descripcion).HasColumnName(nameof(Empadronamiento.Descripcion)).HasMaxLength(50).IsRequired();
        }
    }
}
