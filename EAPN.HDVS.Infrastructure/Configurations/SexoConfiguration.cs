using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class SexoConfiguration : IEntityTypeConfiguration<Sexo>
    {
        public void Configure(EntityTypeBuilder<Sexo> builder)
        {
            builder.ToTable("Sexos", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(nameof(Sexo.Id)).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Descripcion).HasColumnName(nameof(Sexo.Descripcion)).HasMaxLength(50).IsRequired();

            builder.HasData(
                new Sexo { Id = 1, Descripcion = "Hombre" },
                new Sexo { Id = 2, Descripcion = "Mujer" },
                new Sexo { Id = 3, Descripcion = "Otros" }
            );
        }
    }
}
