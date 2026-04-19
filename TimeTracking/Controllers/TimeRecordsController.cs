using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Models;

namespace Controllers
{
    /// <summary>
    /// API для учета рабочего времени.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TimeRecordsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Инициализирует новый экземпляр контроллера.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        public TimeRecordsController(ApplicationDbContext context) 
            => _context = context;

        /// <summary>
        /// Возвращает записи времени с фильтрацией по дате или периоду.
        /// </summary>
        /// <remarks>
        /// (StatusColor):
        /// Green: ровно 8 часов.
        /// Yellow: меньше 8 часов.
        /// Red: больше 8 часов.
        /// </remarks>
        /// <param name="date">Конкретная дата.</param>
        /// <param name="month">Месяц.</param>
        /// <param name="year">Год.</param>
        /// <returns>Объект с записями, суммой часов и цветовым статусом.</returns>
        [HttpGet]
        public async Task<ActionResult> GetRecords(
            [FromQuery] DateTime? date, [FromQuery] int? month, 
            [FromQuery] int? year)
        {
            var query = _context.TimeRecords.AsQueryable();

            if (date.HasValue)
            {
                query = query
                    .Where(r => r.Date == date.Value.Date);
            }

            else if (month.HasValue && year.HasValue)
            {
                query = query.Where
                    (r => r.Date.Month == month
                    && r.Date.Year == year);
            }

            var records = await query.Include(r => r.WorkTask)
                .ToListAsync();

            
            var totalHours = records.Sum(r => r.Hours);
            string color = totalHours < 8 ? "Yellow" 
                : (totalHours == 8 ? "Green" : "Red");

            return Ok(new 
            { 
                Records = records, 
                TotalHours = totalHours, 
                StatusColor = color 
            });
        }

        /// <summary>
        /// Возвращает список записей за указанный месяц и год.
        /// </summary>
        /// <param name="year">Год.</param>
        /// <param name="month">Месяц.</param>
        /// <returns>Список записей и общее количество часов за месяц.</returns>
        [HttpGet("month/{year}/{month}")]
        public async Task<ActionResult> GetMonthRecords(int year,
            int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var records = await _context.TimeRecords
                .Include(tr => tr.WorkTask)
                .Where(tr => tr.Date >= startDate && tr.Date <= endDate)
                .OrderBy(tr => tr.Date)
                .ToListAsync();

            var totalHours = records.Sum(r => r.Hours);

            return Ok(new
            {
                records = records.Select(r => new {
                    r.Id,
                    r.Date,
                    r.Hours,
                    r.Description,
                    WorkTask = new { Name = r.WorkTask!.Name}
                }),
                totalMonthHours = totalHours
            });
        }

        /// <summary>
        /// Создает запись о затраченном времени.
        /// </summary>
        /// <param name="dto">Данные записи времени.</param>
        /// <returns>Созданная запись или 400 (Bad Request) при нарушении лимитов или неактивной задаче.</returns>
        [HttpPost]
        public async Task<ActionResult> CreateRecord(RecordCreateDto dto)
        {
            var task = await _context.WorkTasks.FindAsync(dto.WorkTaskId);

            // Проверка активности задачи.
            if (task == null || !task.IsActive)
            { 
                return BadRequest("Нельзя списать время " +
                   "на неактивную или несуществующую задачу.");
            }


            var existingHours = await _context.TimeRecords
                .Where(r => r.Date == dto.Date.Date)
                .SumAsync(r => r.Hours);

            // Проверка суточного лимита.
            if (existingHours + dto.Hours > 24)
            {
                return BadRequest(
                    $"Превышен лимит 24 часа. " +
                    $"Уже введено: {existingHours}");
            }

            var record = new TimeRecord
            {
                Date = dto.Date.Date,
                Hours = dto.Hours,
                Description = dto.Description,
                WorkTaskId = dto.WorkTaskId
            };

            _context.TimeRecords.Add(record);
            await _context.SaveChangesAsync();
            return Ok(record);
        }
    }

    /// <summary>
    /// Данные для создания записи учета времени.
    /// </summary>
    public class RecordCreateDto
    {
        /// <summary> Дата выполнения работ.</summary>
        public DateTime Date { get; set; }

        /// <summary> Количество затраченных часов.</summary>
        public decimal Hours { get; set; }

        /// <summary> Описание выполненных работ.</summary>
        public string Description { get; set; } = string.Empty;

        /// <summary> Идентификатор рабочей задачи.</summary>
        public int WorkTaskId { get; set; }
    }
}
