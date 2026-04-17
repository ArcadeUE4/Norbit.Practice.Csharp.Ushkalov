using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using DTOs;
using Models;

namespace Controllers
{
    /// <summary>
    /// Контроллер для управления проектами.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получает все проекты по дате и месяцу.
        /// </summary>
        /// <returns>Асихронная операция, возращает<see cref="ActionResult{T}"/> 
        /// содержает коллекцию всех
        /// <see cref="Project"/>. Иначе возращает пустую 
        /// коллецию, если пусто .</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            return await _context.Projects.ToListAsync();
        }

        /// <summary>
        /// Извлекает проект с указанным идентификатором.
        /// </summary>
        /// <param name="id">Уникальный идентификатор 
        /// проекта для извлечения.</param>
        /// <returns>ActionResult
        /// иначе NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = 
                await _context.Projects.FindAsync(id);

            if (project == null) return NotFound();

            return project;
        }

        /// <summary>
        /// Создает новый проект, используя данные проекта
        /// </summary>
        /// <remarks>Код 201</remarks> 
        /// <param name="projectDto">Объект передачи, 
        /// содеращий сведения о проекте</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        public async Task<ActionResult<Project>> CreateProject(ProjectDto projectDto)
        {
            var project = new Project
            {
                Name = projectDto.Name,
                Code = projectDto.Code,
                IsActive = projectDto.IsActive
            };

            _context.Projects.Add(project);
            await _context.Set<Project>().AddAsync(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProject), new 
            { 
                id = project.Id 
            }, 
            project);
        }

        /// <summary>
        ///  Обновляет сведения о существующем проекте с указанным идентификатором.
        /// </summary>
        /// <param name="id">Уникальный индефикатор для обновления.</param>
        /// <param name="projectDto">Объект содержащий всю инофрмацию о проекте.</param>
        /// <returns>NoContent если успешно;
        /// NotFound, если проекта не существует.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject
            (int id, ProjectDto projectDto)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null) return NotFound();

            project.Name = projectDto.Name;
            project.Code = projectDto.Code;
            project.IsActive = projectDto.IsActive;

            try
            {
                await _context.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Удалает проект с указанным идентификатором.
        /// </summary>
        /// <param name="id">Уникальный номер для удаления проекта.</param>
        /// <returns>NoContent если успешно;
        /// NotFound, если проекта не существует.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject
            (int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null) return NotFound();

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(
                e => e.Id == id);
        }
    }
}
