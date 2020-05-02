using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(nameof(RefreshToken.Id)).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.UsuarioId).HasColumnName(nameof(RefreshToken.UsuarioId)).IsRequired();
            builder.Property(x => x.Token).HasColumnName(nameof(RefreshToken.Token)).HasMaxLength(128).IsRequired();
            builder.Property(x => x.FinValidez).HasColumnName(nameof(RefreshToken.FinValidez)).IsRequired();

            builder.HasOne(x => x.Usuario).WithMany(x => x.Tokens).HasForeignKey(x => x.UsuarioId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
