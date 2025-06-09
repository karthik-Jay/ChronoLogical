using ChronoLogical.Api.Data;
using ChronoLogical.Api.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChronoLogical.Api.Controllers
{
    /// <summary>
    /// API controller for managing time entries.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TimeEntriesController : ControllerBase
    {
        private readonly ChronoLogicalContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeEntriesController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public TimeEntriesController(ChronoLogicalContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all time entries.
        /// </summary>
        /// <returns>A list of all <see cref="TimeEntry"/> objects.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TimeEntry>>> GetTimeEntries()
        {
            return await _context.TimeEntries.ToListAsync();
        }

        /// <summary>
        /// Gets all time entries for the week containing the specified date.
        /// </summary>
        /// <param name="date">A date within the desired week.</param>
        /// <returns>A list of <see cref="TimeEntry"/> objects for the specified week.</returns>
        [HttpGet("week/{date:datetime}")]
        public async Task<ActionResult<IEnumerable<TimeEntry>>> GetWeekTimeEntries(DateTime date)
        {
            var startOfWeek = date.AddDays(-(int)date.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(7);

            var timeEntries = await _context.TimeEntries
                .Where(te => te.StartTime >= startOfWeek && te.StartTime < endOfWeek)
                .OrderBy(te => te.StartTime)
                .ToListAsync();

            return timeEntries;
        }

        /// <summary>
        /// Gets a specific time entry by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the time entry.</param>
        /// <returns>The <see cref="TimeEntry"/> with the specified identifier, or NotFound if not found.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TimeEntry>> GetTimeEntry(int id)
        {
            var timeEntry = await _context.TimeEntries.FindAsync(id);

            if (timeEntry == null)
            {
                return NotFound();
            }

            return timeEntry;
        }

        /// <summary>
        /// Creates a new time entry.
        /// </summary>
        /// <param name="timeEntry">The <see cref="TimeEntry"/> to create.</param>
        /// <returns>The created <see cref="TimeEntry"/> with its new identifier.</returns>
        [HttpPost]
        public async Task<ActionResult<TimeEntry>> CreateTimeEntry(TimeEntry timeEntry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (timeEntry.EndTime <= timeEntry.StartTime)
            {
                return BadRequest("End time must be after start time");
            }

            _context.TimeEntries.Add(timeEntry);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTimeEntry), new { id = timeEntry.Id }, timeEntry);
        }

        /// <summary>
        /// Updates an existing time entry.
        /// </summary>
        /// <param name="id">The identifier of the time entry to update.</param>
        /// <param name="timeEntry">The updated <see cref="TimeEntry"/> object.</param>
        /// <returns>No content if successful, or an error response.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTimeEntry(int id, TimeEntry timeEntry)
        {
            if (id != timeEntry.Id)
            {
                return BadRequest("ID mismatch");
            }

            if (timeEntry.EndTime <= timeEntry.StartTime)
            {
                return BadRequest("End time must be after start time");
            }

            _context.Entry(timeEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TimeEntryExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes a time entry by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the time entry to delete.</param>
        /// <returns>No content if successful, or NotFound if the entry does not exist.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTimeEntry(int id)
        {
            var timeEntry = await _context.TimeEntries.FindAsync(id);
            if (timeEntry == null)
            {
                return NotFound();
            }

            _context.TimeEntries.Remove(timeEntry);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Determines whether a time entry exists with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier to check.</param>
        /// <returns><c>true</c> if the time entry exists; otherwise, <c>false</c>.</returns>
        private bool TimeEntryExists(int id)
        {
            return _context.TimeEntries.Any(e => e.Id == id);
        }
    }
}