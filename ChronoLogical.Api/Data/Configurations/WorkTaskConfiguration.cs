using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChronoLogical.Api.Model;

namespace ChronoLogical.Api.Data.Configurations
{
    /// <summary>
    /// Entity Framework configuration for the Task entity.
    /// </summary>
    public class WorkTaskConfiguration : IEntityTypeConfiguration<WorkTask>
    {
        public void Configure(EntityTypeBuilder<WorkTask> builder)
        {
            // Table name
            builder.ToTable("Tasks");

            builder.HasKey(t => t.Id);
            builder.Property(te => te.Id).ValueGeneratedOnAdd();

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.DevopsId)
                .HasMaxLength(50);

            builder.HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}   