﻿using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class PerfilConfiguration : IEntityTypeConfiguration<Perfil>
    {
        public void Configure(EntityTypeBuilder<Perfil> builder)
        {
            builder.ToTable("Perfiles", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(nameof(Perfil.Id)).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Descripcion).HasColumnName(nameof(Perfil.Descripcion)).HasMaxLength(50).IsRequired();

            builder.HasMany(x => x.Permisos).WithOne(x => x.Perfil).HasForeignKey(x => x.PerfilId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Usuarios).WithOne(x => x.Perfil).HasForeignKey(x => x.PerfilId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
