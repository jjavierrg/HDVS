using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class AdjuntoConfiguration : IEntityTypeConfiguration<Adjunto>
    {
        public void Configure(EntityTypeBuilder<Adjunto> builder)
        {
            builder.ToTable("Adjuntos", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(nameof(Adjunto.Id)).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.TipoId).HasColumnName(nameof(Adjunto.TipoId)).IsRequired();
            builder.Property(x => x.UsuarioId).HasColumnName(nameof(Adjunto.UsuarioId));
            builder.Property(x => x.FichaId).HasColumnName(nameof(Adjunto.FichaId));
            builder.Property(x => x.OrganizacionId).HasColumnName(nameof(Adjunto.OrganizacionId));
            builder.Property(x => x.Alias).HasColumnName(nameof(Adjunto.Alias)).HasMaxLength(255).IsRequired();
            builder.Property(x => x.NombreOriginal).HasColumnName(nameof(Adjunto.NombreOriginal)).HasMaxLength(255).IsRequired();
            builder.Property(x => x.Tamano).HasColumnName(nameof(Adjunto.Tamano));
            builder.Property(x => x.FechaAlta).HasColumnName(nameof(Adjunto.FechaAlta)).IsRequired();

            builder.Ignore(x => x.FullPath);

            builder.HasOne(x => x.Tipo).WithMany().HasForeignKey(x => x.TipoId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Ficha).WithMany(x => x.Adjuntos).HasForeignKey(x => x.FichaId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
