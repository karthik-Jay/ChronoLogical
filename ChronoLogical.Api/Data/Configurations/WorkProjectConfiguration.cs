using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChronoLogical.Api.Model;

namespace ChronoLogical.Api.Data.Configurations
{
    /// <summary>
    /// Entity Framework configuration for the Project entity.
    /// </summary>
    public class WorkProjectConfiguration : IEntityTypeConfiguration<WorkProject>
    {
        public void Configure(EntityTypeBuilder<WorkProject> builder)
        {
            builder.ToTable("Projects");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.ProjectCode)
                   .HasMaxLength(50);

            builder.HasMany(p => p.Tasks)
                   .WithOne(t => t.Project)
                   .HasForeignKey(t => t.ProjectId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}