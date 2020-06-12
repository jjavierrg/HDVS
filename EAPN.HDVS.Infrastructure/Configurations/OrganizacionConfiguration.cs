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
            builder.Property(x => x.FotoId).HasColumnName(nameof(Ficha.FotoId));
            builder.Property(x => x.Nombre).HasColumnName(nameof(Organizacion.Nombre)).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Email).HasColumnName(nameof(Organizacion.Email)).HasMaxLength(250);
            builder.Property(x => x.Telefono).HasColumnName(nameof(Organizacion.Telefono)).HasMaxLength(20);
            builder.Property(x => x.Web).HasColumnName(nameof(Organizacion.Web)).HasMaxLength(250);
            builder.Property(x => x.Activa).HasColumnName(nameof(Organizacion.Activa)).IsRequired();
            builder.Property(x => x.Observaciones).HasColumnName(nameof(Organizacion.Observaciones));
            builder.Property(x => x.FechaAlta).HasColumnName(nameof(Organizacion.FechaAlta));

            builder.HasOne(x => x.Foto).WithOne(x => x.FotoOrganizacion).HasForeignKey<Organizacion>(x => x.FotoId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(x => x.Usuarios).WithOne(x => x.Organizacion).HasForeignKey(x => x.OrganizacionId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
