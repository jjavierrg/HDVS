using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class FichaConfiguration : IEntityTypeConfiguration<Ficha>
    {
        public void Configure(EntityTypeBuilder<Ficha> builder)
        {
            builder.ToTable("Fichas", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(nameof(Ficha.Id)).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.FotoId).HasColumnName(nameof(Ficha.FotoId));
            builder.Property(x => x.OrganizacionId).HasColumnName(nameof(Ficha.OrganizacionId)).IsRequired();
            builder.Property(x => x.UsuarioId).HasColumnName(nameof(Ficha.UsuarioId)).IsRequired();
            builder.Property(x => x.Codigo).HasColumnName(nameof(Ficha.Codigo)).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Nombre).HasColumnName(nameof(Ficha.Nombre)).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Apellido1).HasColumnName(nameof(Ficha.Apellido1)).HasMaxLength(250);
            builder.Property(x => x.Apellido2).HasColumnName(nameof(Ficha.Apellido2)).HasMaxLength(250);
            builder.Property(x => x.DNI).HasColumnName(nameof(Ficha.DNI)).HasMaxLength(12);
            builder.Property(x => x.FotocopiaDNI).HasColumnName(nameof(Ficha.FotocopiaDNI));
            builder.Property(x => x.FechaNacimiento).HasColumnName(nameof(Ficha.FechaNacimiento));
            builder.Property(x => x.SexoId).HasColumnName(nameof(Ficha.SexoId));
            builder.Property(x => x.GeneroId).HasColumnName(nameof(Ficha.GeneroId));
            builder.Property(x => x.Domicilio).HasColumnName(nameof(Ficha.Domicilio)).HasMaxLength(250);
            builder.Property(x => x.CP).HasColumnName(nameof(Ficha.CP)).HasMaxLength(10);
            builder.Property(x => x.MunicipioId).HasColumnName(nameof(Ficha.MunicipioId));
            builder.Property(x => x.ProvinciaId).HasColumnName(nameof(Ficha.ProvinciaId));
            builder.Property(x => x.PadronId).HasColumnName(nameof(Ficha.PadronId));
            builder.Property(x => x.DocumentacionEmpadronamiento).HasColumnName(nameof(Ficha.DocumentacionEmpadronamiento));
            builder.Property(x => x.NacionalidadId).HasColumnName(nameof(Ficha.NacionalidadId));
            builder.Property(x => x.OrigenId).HasColumnName(nameof(Ficha.OrigenId));
            builder.Property(x => x.SituacionAdministrativaId).HasColumnName(nameof(Ficha.SituacionAdministrativaId));
            builder.Property(x => x.Telefono).HasColumnName(nameof(Ficha.Telefono)).HasMaxLength(20);
            builder.Property(x => x.Email).HasColumnName(nameof(Ficha.Email)).HasMaxLength(255);
            builder.Property(x => x.MotivoAlta).HasColumnName(nameof(Ficha.MotivoAlta));
            builder.Property(x => x.Observaciones).HasColumnName(nameof(Ficha.Observaciones));
            builder.Property(x => x.PoliticaFirmada).HasColumnName(nameof(Ficha.PoliticaFirmada));
            builder.Property(x => x.Completa).HasColumnName(nameof(Ficha.Completa));
            builder.Property(x => x.FechaAlta).HasColumnName(nameof(Ficha.FechaAlta));

            builder.Ignore(x => x.Edad);

            builder.HasOne(x => x.Foto).WithMany().HasForeignKey(x => x.FotoId).OnDelete(DeleteBehavior.ClientCascade);
            builder.HasOne(x => x.Organizacion).WithMany().HasForeignKey(x => x.OrganizacionId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Tecnico).WithMany().HasForeignKey(x => x.UsuarioId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Sexo).WithMany().HasForeignKey(x => x.SexoId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Genero).WithMany().HasForeignKey(x => x.GeneroId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Municipio).WithMany().HasForeignKey(x => x.MunicipioId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Provincia).WithMany().HasForeignKey(x => x.ProvinciaId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Padron).WithMany().HasForeignKey(x => x.PadronId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Nacionalidad).WithMany().HasForeignKey(x => x.NacionalidadId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Origen).WithMany().HasForeignKey(x => x.OrigenId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.SituacionAdministrativa).WithMany().HasForeignKey(x => x.SituacionAdministrativaId).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(x => x.Seguimientos).WithOne(x => x.Ficha).HasForeignKey(x => x.FichaId).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(x => x.Adjuntos).WithOne(x => x.Ficha).HasForeignKey(x => x.FichaId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
