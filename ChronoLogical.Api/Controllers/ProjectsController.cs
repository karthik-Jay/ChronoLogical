using ChronoLogical.Api.Data;
using ChronoLogical.Api.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChronoLogical.Api.Controllers
{
    /// <summary>
    /// API controller for managing projects and their related tasks.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly ChronoLogicalContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectsController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public ProjectsController(ChronoLogicalContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all projects with their related tasks.
        /// </summary>
        /// <returns>A list of all <see cref="WorkProject"/> objects with tasks.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkProject>>> GetProjects()
        {
            return await _context.Projects
                                 .Include(p => p.Tasks)
                                 .ToListAsync();
        }

        /// <summary>
        /// Gets a specific project by its identifier, including its tasks.
        /// </summary>
        /// <param name="id">The identifier of the project.</param>
        /// <returns>The <see cref="WorkProject"/> with the specified identifier, or NotFound if not found.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkProject>> GetProject(int id)
        {
            var project = await _context.Projects
                                        .Include(p => p.Tasks)
                                        .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="project">The <see cref="WorkProject"/> to create.</param>
        /// <returns>The created <see cref="WorkProject"/> with its new identifier.</returns>
        [HttpPost]
        public async Task<ActionResult<WorkProject>> CreateProject(WorkProject project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            // Load tasks if any were included
            await _context.Entry(project).Collection(p => p.Tasks).LoadAsync();

            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }

        /// <summary>
        /// Updates an existing project.
        /// </summary>
        /// <param name="id">The identifier of the project to update.</param>
        /// <param name="project">The updated <see cref="WorkProject"/> object.</param>
        /// <returns>No content if successful, or an error response.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, WorkProject project)
        {
            if (id != project.Id)
            {
                return BadRequest("ID mismatch");
            }

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Gets all tasks for a specific project.
        /// </summary>
        /// <param name="projectId">The identifier of the project.</param>
        /// <returns>A list of <see cref="WorkTask"/> objects for the specified project.</returns>
        [HttpGet("{projectId}/tasks")]
        public async Task<ActionResult<IEnumerable<WorkTask>>> GetTasksForProject(int projectId)
        {
            var project = await _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
            {
                return NotFound();
            }

            return Ok(project.Tasks);
        }

        /// <summary>
        /// Adds a new task to a specific project.
        /// </summary>
        /// <param name="projectId">The identifier of the project.</param>
        /// <param name="task">The <see cref="WorkTask"/> to add.</param>
        /// <returns>The created <see cref="WorkTask"/>.</returns>
        [HttpPost("{projectId}/tasks")]
        public async Task<ActionResult<WorkTask>> AddTaskToProject(int projectId, WorkTask task)
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null)
            {
                return NotFound("Project not found.");
            }

            task.ProjectId = projectId;
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { projectId = projectId, taskId = task.Id }, task);
        }

        /// <summary>
        /// Gets a specific task by its identifier within a project.
        /// </summary>
        /// <param name="projectId">The identifier of the project.</param>
        /// <param name="taskId">The identifier of the task.</param>
        /// <returns>The <see cref="WorkTask"/> if found, otherwise NotFound.</returns>
        [HttpGet("{projectId}/tasks/{taskId}")]
        public async Task<ActionResult<WorkTask>> GetTask(int projectId, int taskId)
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == taskId && t.ProjectId == projectId);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        /// <summary>
        /// Updates a specific task within a project.
        /// </summary>
        /// <param name="projectId">The identifier of the project.</param>
        /// <param name="taskId">The identifier of the task.</param>
        /// <param name="task">The updated <see cref="WorkTask"/> object.</param>
        /// <returns>No content if successful, or an error response.</returns>
        [HttpPut("{projectId}/tasks/{taskId}")]
        public async Task<IActionResult> UpdateTask(int projectId, int taskId, WorkTask task)
        {
            if (taskId != task.Id || projectId != task.ProjectId)
            {
                return BadRequest("ID mismatch");
            }

            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(taskId, projectId))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }

        private bool TaskExists(int taskId, int projectId)
        {
            return _context.Tasks.Any(t => t.Id == taskId && t.ProjectId == projectId);
        }
    }
}