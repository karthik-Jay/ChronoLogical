﻿using Microsoft.EntityFrameworkCore;

namespace ChronoLogical.Api.Data
{
    public class ChronoLogicalContext : DbContext
    {
        public ChronoLogicalContext(DbContextOptions<ChronoLogicalContext> options)
            : base(options)
        {
        }

        public DbSet<Model.TimeEntry> TimeEntries { get; set; } = null!;
        public DbSet<Model.WorkProject> Projects { get; set; } = null!;
        public DbSet<Model.WorkTask> Tasks { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChronoLogicalContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
