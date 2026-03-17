using Microsoft.EntityFrameworkCore;
using GameStoreConsole.Models;

namespace GameStoreConsole.Data
{
    public class GameStoreContext : DbContext
    {
         private readonly string _connectionString;

         public GameStoreContext(string connectionString) 
            => _connectionString = connectionString;

         public DbSet<Game> Games { get; set; }
         public DbSet<Studio> Studios { get; set; }
         public DbSet<Player> Players { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
         {
           optionsBuilder.UseSqlServer(_connectionString);
         }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Предотвращает случайное удаление студий, если есть игры
            modelBuilder.Entity<Game>()
                .HasOne(g => g.Studio)
                .WithMany(s => s.Games)
                .HasForeignKey(g => g.StudioID)
                .OnDelete(DeleteBehavior.Restrict); 
        }
    }

}