using Microsoft.EntityFrameworkCore;
using Models;

namespace Data
{
    /// <summary>
    /// Контекст базы для управления сущности учета времени.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options) { }


        public DbSet<Project> Projects { get; set; }
        public DbSet<WorkTask> WorkTasks { get; set; }
        public DbSet<TimeRecord> TimeRecords { get; set; }


        //Построение модели.
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
