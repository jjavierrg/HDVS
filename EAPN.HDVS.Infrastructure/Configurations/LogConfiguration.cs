using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPN.HDVS.Infrastructure.Configurations
{
    public class LogConfiguration : IEntityTypeConfiguration<LogEntry>
    {
        public void Configure(EntityTypeBuilder<LogEntry> builder)
        {
            builder.ToTable("Logs", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(nameof(LogEntry.Id)).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.UserId).HasColumnName(nameof(LogEntry.UserId));
            builder.Property(x => x.CallSite).HasColumnName(nameof(LogEntry.CallSite));
            builder.Property(x => x.Date).HasColumnName(nameof(LogEntry.Date));
            builder.Property(x => x.Exception).HasColumnName(nameof(LogEntry.Exception));
            builder.Property(x => x.Level).HasColumnName(nameof(LogEntry.Level));
            builder.Property(x => x.Logger).HasColumnName(nameof(LogEntry.Logger));
            builder.Property(x => x.Message).HasColumnName(nameof(LogEntry.Message));
            builder.Property(x => x.LevelOrder).HasColumnName(nameof(LogEntry.LevelOrder));
            builder.Property(x => x.Ip).HasColumnName(nameof(LogEntry.Ip));

            builder.HasOne(x => x.Usuario).WithMany().HasForeignKey(x => x.UserId);
        }
    }
}
