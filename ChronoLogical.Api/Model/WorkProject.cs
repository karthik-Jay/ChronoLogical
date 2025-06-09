using System.Collections.Generic;

namespace ChronoLogical.Api.Model
{
    /// <summary>
    /// Represents a project, which can have multiple tasks.
    /// </summary>
    public class WorkProject
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string ProjectCode { get; set; } = default!;

        /// <summary>
        /// Collection of tasks associated with this project.
        /// </summary>
        public ICollection<WorkTask> Tasks { get; set; } = new List<WorkTask>();
    }
}