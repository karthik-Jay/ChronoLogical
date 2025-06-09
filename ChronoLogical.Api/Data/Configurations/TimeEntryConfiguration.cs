using ChronoLogical.Api.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronoLogical.Api.Data.Configurations
{
    public class TimeEntryConfiguration : IEntityTypeConfiguration<TimeEntry>
    {
        public void Configure(EntityTypeBuilder<TimeEntry> builder)
        {
            // Table name
            builder.ToTable("TimeEntries");

            // PK
            builder.HasKey(te => te.Id);
            builder.Property(te => te.Id).ValueGeneratedOnAdd();

            // Required fields
            builder.Property(te => te.StartTime)
                   .IsRequired()
                   .HasColumnType("datetime2");

            builder.Property(te => te.EndTime)
                   .IsRequired()
                   .HasColumnType("datetime2");

            // optional string fields with length constraints
            builder.Property(te => te.Description)
                   .HasMaxLength(500)
                   .IsUnicode(true)
                   .IsRequired(false);

            builder.Property(te => te.ProjectCode)
                   .HasMaxLength(50)
                   .IsUnicode(false)
                   .IsRequired(true);

            builder.Property(te => te.TaskId)
                   .HasMaxLength(50)
                   .IsUnicode(false);

            // indexes for performance
            builder.HasIndex(e => e.StartTime)
                  .HasDatabaseName("IX_TimeEntries_StartTime");

            builder.HasIndex(e => e.ProjectCode)
                   .HasDatabaseName("IX_TimeEntries_ProjectCode");

            builder.HasIndex(e => e.TaskId)
                   .HasDatabaseName("IX_TimeEntries_TaskId");

            // Composite index for date range queries
            builder.HasIndex(e => new { e.StartTime, e.EndTime })
                   .HasDatabaseName("IX_TimeEntries_DateRange");
        }
    }
}
