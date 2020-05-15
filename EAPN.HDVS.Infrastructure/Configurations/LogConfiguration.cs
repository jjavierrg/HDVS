using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class LogConfiguration : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.ToTable("Logs", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(nameof(Log.Id)).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.UserId).HasColumnName(nameof(Log.UserId));
            builder.Property(x => x.CallSite).HasColumnName(nameof(Log.CallSite));
            builder.Property(x => x.Date).HasColumnName(nameof(Log.Date));
            builder.Property(x => x.Exception).HasColumnName(nameof(Log.Exception));
            builder.Property(x => x.Level).HasColumnName(nameof(Log.Level));
            builder.Property(x => x.Logger).HasColumnName(nameof(Log.Logger));
            builder.Property(x => x.Message).HasColumnName(nameof(Log.Message));
            builder.Property(x => x.LevelOrder).HasColumnName(nameof(Log.LevelOrder));
            builder.Property(x => x.Ip).HasColumnName(nameof(Log.Ip));
        }
    }
}
