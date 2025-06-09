namespace ChronoLogical.Api.Model
{
    public class TimeEntry
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Description { get; set; }
        public string? ProjectCode { get; set; }
        public string? TaskId { get; set; }

        // Calculated property for duration in hours
        public decimal Duration => (decimal)(EndTime - StartTime).TotalHours;
    }
}
