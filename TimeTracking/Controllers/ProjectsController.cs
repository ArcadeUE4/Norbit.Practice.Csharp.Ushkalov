using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using DTOs;
using Models;

namespace Controllers
{
    /// <summary>
    /// API для управления проектами.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Иннициализирует новый экземпляр контроллера.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Возращает список всех проектов.
        /// </summary>
        /// <returns>Коллекцию объектов<see cref="Project"/></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            return await _context.Projects.ToListAsync();
        }

        /// <summary>
        /// Возрашает проект по его идентификатору.
        /// </summary>
        /// <param name="id">Индефикатор проекта.</param>
        /// <returns>Объект проекта или ошибка 404.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = 
                await _context.Projects.FindAsync(id);

            if (project == null) return NotFound();

            return project;
        }

        /// <summary>
        /// Создает новый проект.
        /// </summary>
        /// <param name="projectDto">Данные для создание проекта</param>
        /// <returns>Созданный объект проекта с входом 201.</returns>
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
        /// Обновляет существующий проект.
        /// </summary>
        /// <param name="id">Идефикатор проекта.</param>
        /// <param name="projectDto">Новые данные проекта.</param>
        /// <returns>204 при успехе или 404.</returns>
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
        /// Удалает проект.
        /// </summary>
        /// <param name="id">Иденфикатор проекта.</param>
        /// <returns>204 при успехе или 404.</returns>
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
