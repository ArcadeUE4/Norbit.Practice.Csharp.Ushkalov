using Microsoft.EntityFrameworkCore;
using Models;

namespace Data
{
    /// <summary>
    /// Контекст базы для управления системы учета времени.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Инициализирует новый экземпляр контекста.
        /// </summary>
        /// <param name="options">Параметры конфиграции контекста.</param>
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        /// <summary>
        /// Набор данных проектов.
        /// </summary>
        public DbSet<Project> Projects { get; set; }

        /// <summary>
        /// Набор данных рабочих задач.
        /// </summary>
        public DbSet<WorkTask> WorkTasks { get; set; }

        /// <summary>
        /// Набор данных записей учета времени.
        /// </summary>
        public DbSet<TimeRecord> TimeRecords { get; set; }

        /// <summary>
        /// Настраивает конфигурацию моделей при их создании.
        /// </summary>
        /// <param name="modelBuilder">Построитель моделей для настройки сущностей.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TimeRecord>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("date");
                entity.Property(e => e.Hours).HasPrecision(4, 2);
            });

            modelBuilder.Entity<WorkTask>()
                .HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId);
        }
    }
}
