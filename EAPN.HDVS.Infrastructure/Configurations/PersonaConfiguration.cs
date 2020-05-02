using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class PersonaConfiguration : IEntityTypeConfiguration<Persona>
    {
        public void Configure(EntityTypeBuilder<Persona> builder)
        {
            builder.ToTable("Personas", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(nameof(Persona.Id)).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Codigo).HasColumnName(nameof(Persona.Codigo)).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Nombre).HasColumnName(nameof(Persona.Nombre)).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Apellido1).HasColumnName(nameof(Persona.Apellido1)).HasMaxLength(250);
            builder.Property(x => x.Apellido2).HasColumnName(nameof(Persona.Apellido2)).HasMaxLength(250);
            builder.Property(x => x.DNI).HasColumnName(nameof(Persona.DNI)).HasMaxLength(12);
            builder.Property(x => x.FechaNacimiento).HasColumnName(nameof(Persona.FechaNacimiento));
            builder.Property(x => x.SexoId).HasColumnName(nameof(Persona.SexoId)).IsRequired();
            builder.Property(x => x.SexoId).HasColumnName(nameof(Persona.NacionalidadId)).IsRequired();
            builder.Property(x => x.DocumentacionEmpadronamiento).HasColumnName(nameof(Persona.DocumentacionEmpadronamiento)).IsRequired();

            builder.Ignore(x => x.Edad);

            builder.HasMany(x => x.Direcciones).WithOne(x => x.Persona).HasForeignKey(x => x.PersonaId);
            builder.HasOne(x => x.Sexo).WithMany().HasForeignKey(x => x.SexoId);
            builder.HasOne(x => x.Nacionalidad).WithMany().HasForeignKey(x => x.NacionalidadId);
        }
    }
}
