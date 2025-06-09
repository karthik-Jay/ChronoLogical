namespace ChronoLogical.Api.Model
{
    /// <summary>
    /// Represents a task, which belongs to a project.
    /// </summary>
    public class WorkTask
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string DevopsId { get; set; } = default!;

        /// <summary>
        /// Foreign key to the associated project.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Navigation property to the associated project.
        /// </summary>
        public WorkProject Project { get; set; } = default!;
    }
}