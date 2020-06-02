using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class SeguimientoConfiguration : IEntityTypeConfiguration<Seguimiento>
    {
        public void Configure(EntityTypeBuilder<Seguimiento> builder)
        {
            builder.ToTable("Seguimientos", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(nameof(Seguimiento.Id)).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Fecha).HasColumnName(nameof(Seguimiento.Fecha)).IsRequired();
            builder.Property(x => x.UsuarioId).HasColumnName(nameof(Seguimiento.UsuarioId)).IsRequired();
            builder.Property(x => x.FichaId).HasColumnName(nameof(Seguimiento.FichaId)).IsRequired();
            builder.Property(x => x.Observaciones).HasColumnName(nameof(Seguimiento.Observaciones));

            builder.HasOne(x => x.Tecnico).WithMany().HasForeignKey(x => x.UsuarioId).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(x => x.Ficha).WithMany(x => x.Seguimientos).HasForeignKey(x => x.FichaId).OnDelete(DeleteBehavior.Cascade); ;
        }
    }
}
