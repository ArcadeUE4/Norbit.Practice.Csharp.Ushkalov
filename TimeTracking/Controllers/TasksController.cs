using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Models;

    namespace Controllers
    {
        /// <summary>
        /// API для управления рабочими задачами.
        /// </summary>
        [ApiController]
        [Route("api/[controller]")]
        public class TasksController : ControllerBase
        {

            private readonly ApplicationDbContext _context;

            /// <summary>
            /// Инициализирует новый экземпляр контроллера.
            /// </summary>
            /// <param name="context">Контекст базы данных.</param>
            public TasksController(ApplicationDbContext context) 
                        => _context = context;

            /// <summary>
            /// Возвращает список всех задач с информацией о проектах.
            /// </summary>
            /// <returns>Коллекция объектов <see cref="WorkTask"/>.</returns>
            [HttpGet]
            public async Task<ActionResult<IEnumerable<WorkTask>>> GetTasks() => 
                 await _context.WorkTasks
                .Include(t => t.Project).ToListAsync();

            /// <summary>
            /// Создает новую задачу и связывает ее с проектом.
            /// </summary>
            /// <param name="dto">Данные для создания задачи.</param>
            /// <returns>Созданная задача или 400 (Bad Request), если проект не найден.</returns>
            [HttpPost]
            public async Task<ActionResult<WorkTask>> 
                CreateTask(TaskCreateDto dto)
            {
                // Проверяем, существует ли проект.
                var project = await _context.Projects.
                    FindAsync(dto.ProjectId);

                if (project == null)
                {
                   return BadRequest("Указанный проект не найден.");
                }

                var task = new WorkTask
                {
                    Name = dto.Name,
                    ProjectId = dto.ProjectId,
                    IsActive = dto.IsActive
                };

                _context.WorkTasks.Add(task);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    id = task.Id,
                    name = task.Name,
                    project = task.ProjectId,
                    isActive = task.IsActive
                });
            }
        }

        /// <summary>
        /// Данные для создания новой задачи.
        /// </summary>
        public class TaskCreateDto
        {
            /// <summary>Название задачи.</summary>
            public string Name { get; set; } = string.Empty;
            
            /// <summary>Идефикатор связанного проекта.</summary>
            public int ProjectId { get; set; }
            
            /// <summary>Флаг активности задачи.</summary>
            public bool IsActive { get; set; } = true;
        }
    }
