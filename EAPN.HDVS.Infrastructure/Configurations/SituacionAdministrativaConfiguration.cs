using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class SituacionAdministrativaConfiguration : IEntityTypeConfiguration<SituacionAdministrativa>
    {
        public void Configure(EntityTypeBuilder<SituacionAdministrativa> builder)
        {
            builder.ToTable("SituacionesAdministrativas", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(nameof(SituacionAdministrativa.Id)).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Descripcion).HasColumnName(nameof(SituacionAdministrativa.Descripcion)).HasMaxLength(50).IsRequired();
        }
    }
}
