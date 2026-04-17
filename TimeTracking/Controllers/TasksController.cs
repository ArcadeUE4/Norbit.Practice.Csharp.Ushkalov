using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Models;

    namespace Controllers
    {
        /// <summary>
        /// Представялет API для управления рабочими задачами.
        /// </summary>
        [Route("api/[controller]")]
        [ApiController]
        public class TasksController : ControllerBase
        {

            private readonly ApplicationDbContext _context;
            public TasksController(ApplicationDbContext context) 
                => _context = context;
            /// <summary>
            /// Извлекает все рабочие задачи, включая связанную с ними информацию о проекте.
            /// </summary>
            /// <returns><see cref="WorkTask"/>. 
            /// Коллекция будет пустой, если задачи не найдены.</returns>
            [HttpGet]
            public async Task<ActionResult<IEnumerable<WorkTask>>> 
                GetTasks() => await _context.WorkTasks
                .Include(t => t.Project).ToListAsync();

            /// <summary>
            /// Создает новую задачу и связывает ее с проектом.
            /// </summary>
            /// <param name="dto">Объект передачи данных, содержаший информацию о задаче</param>
            /// <returns>200, если удачно, 
            /// 400 - если проекта не существует</returns>
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
        /// Представляет данные, необходимые для проекта.
        /// </summary>
        /// <remarks>Используйте этот объект передачи данных при 
        /// отправке информации для создания задачи. Все свойства
        /// должны быть установлены на допустимые значения перед 
        /// отправкой в ​​API или сервис.</remarks>
        public class TaskCreateDto
        {
            public string Name { get; set; } = string.Empty;
            public int ProjectId { get; set; }
            public bool IsActive { get; set; } = true;
        }
    }
