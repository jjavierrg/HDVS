using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class AsociacionConfiguration : IEntityTypeConfiguration<Asociacion>
    {
        public void Configure(EntityTypeBuilder<Asociacion> builder)
        {
            builder.ToTable("Asociaciones", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(nameof(Asociacion.Id)).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Nombre).HasColumnName(nameof(Asociacion.Nombre)).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Activa).HasColumnName(nameof(Asociacion.Activa)).IsRequired();
            builder.Property(x => x.Observaciones).HasColumnName(nameof(Asociacion.Observaciones));

            builder.HasMany(x => x.Usuarios).WithOne(x => x.Asociacion).HasForeignKey(x => x.AsociacionId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
