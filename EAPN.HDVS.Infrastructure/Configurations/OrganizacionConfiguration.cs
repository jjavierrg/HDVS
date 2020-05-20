using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class OrganizacionConfiguration : IEntityTypeConfiguration<Organizacion>
    {
        public void Configure(EntityTypeBuilder<Organizacion> builder)
        {
            builder.ToTable("Organizaciones", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(nameof(Organizacion.Id)).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Nombre).HasColumnName(nameof(Organizacion.Nombre)).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Activa).HasColumnName(nameof(Organizacion.Activa)).IsRequired();
            builder.Property(x => x.Observaciones).HasColumnName(nameof(Organizacion.Observaciones));

            builder.HasMany(x => x.Usuarios).WithOne(x => x.Organizacion).HasForeignKey(x => x.OrganizacionId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
