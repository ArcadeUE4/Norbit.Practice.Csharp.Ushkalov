using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Models;

namespace Controllers
{
    /// <summary>
    /// Контроллер для учета рабочего времени.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TimeRecordsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public TimeRecordsController(ApplicationDbContext context) 
            => _context = context;

        /// <summary>
        /// Извлекает записи времени, отфильтрованные по определенной дате
        /// или по месяцу и году, и возвращает записи вместе с
        /// общим количеством часов и цветовым индикатором состояния.
        /// </summary>
        /// <remarks>Зеленый, если 8 часов.
        /// Желтый, если меньше 8 часов.
        /// Красный, если больше 8 часов.</remarks>
        /// <param name="date">Дата.</param>
        /// <param name="month">Месяц.</param>
        /// <param name="year">Год.</param>
        /// <returns>200, отфильтрованными записями, общим количеством 
        /// часов для выбранных записей и цветом состояния, 
        /// указывающим на общее количество отработанных часов.</returns>
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
        /// Выдает записи за месяц.
        /// </summary>
        /// <param name="year">Год.</param>
        /// <param name="month">Месяц.</param>
        /// <returns></returns>
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
                    WorkTask = new { r.WorkTask.Name }
                }),
                totalMonthHours = totalHours
            });
        }

        /// <summary>
        /// Создает новую запись времени для указанной рабочей задачи.
        /// </summary>
        /// <remarks>Возращает 400, если превышен лимит 24 часов.
        /// </remarks>
        /// <param name="dto">Объект передачи данных, содержащий 
        /// сведения о создаваемой записи времени.</param>
        /// <returns>200 если успешно, 400 если задача не активна,
        /// не существует или превышен лимит времени.</returns>
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
    /// Представляет данные, необходимые для создания новой 
    /// записи учета рабочего времени.
    /// </summary>
    /// <remarks>Используйте этот объект передачи данных для 
    /// предоставления необходимой информации при создании новой 
    /// записи отработанных часов по 
    /// конкретной задаче. Этот тип обычно используется в 
    /// операциях создания в системах учета рабочего 
    /// времени или системах учета рабочего времени.</remarks>
    public class RecordCreateDto
    {
        public DateTime Date { get; set; }
        public decimal Hours { get; set; }
        public string Description { get; set; } = string.Empty;
        public int WorkTaskId { get; set; }
    }



}
