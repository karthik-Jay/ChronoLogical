using System;

namespace ChronoLogical.Api.Model
{
    /// <summary>
    /// Represents a time entry, which can reference a task or a project.
    /// </summary>
    public class TimeEntry
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Description { get; set; }

        /// <summary>
        /// Optional reference to a project.
        /// </summary>
        public int? ProjectId { get; set; }
        public WorkProject? Project { get; set; }

        /// <summary>
        /// Optional reference to a task.
        /// </summary>
        public int? TaskId { get; set; }
        public WorkTask? Task { get; set; }

        /// <summary>
        /// Calculated property for duration in hours.
        /// </summary>
        public decimal Duration => (decimal)(EndTime - StartTime).TotalHours;
    }
}
