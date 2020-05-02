using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class DireccionConfiguration : IEntityTypeConfiguration<Direccion>
    {
        public void Configure(EntityTypeBuilder<Direccion> builder)
        {
            builder.ToTable("Direcciones", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(nameof(Direccion.Id)).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.PersonaId).HasColumnName(nameof(Direccion.PersonaId)).IsRequired();
            builder.Property(x => x.Descripcion).HasColumnName(nameof(Direccion.Descripcion)).HasMaxLength(100).IsRequired();
            builder.Property(x => x.DireccionCompleta).HasColumnName(nameof(Direccion.DireccionCompleta)).HasMaxLength(500).IsRequired();
            
            builder.HasOne(x => x.Persona).WithMany(x => x.Direcciones).HasForeignKey(x => x.PersonaId);
        }
    }
}
