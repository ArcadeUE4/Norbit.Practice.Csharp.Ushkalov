using Microsoft.EntityFrameworkCore;
using GameStoreConsole.Models;
using GameStoreConsole.Data;

public class EfRepository
{
    private readonly string _conn;
    public EfRepository(string conn) => _conn = conn;

    public void CreateGame(string title, 
        decimal price, 
        Guid studioId)
    {
        try
        {
            using var db = new GameStoreContext(_conn);

            var studioExists = db.Studios.Any(s => s.StudioID == studioId);
            if (!studioExists)
            {
                Console.WriteLine($"Ошибка: Студия с ID " +
                    $"{studioId} не найдена в системе.");
                return;
            }

            var game = new Game { 
                GameID = Guid.NewGuid(), 
                Title = title, 
                Price = price, 
                StudioID = studioId 
            };

            db.Games.Add(game);
            db.SaveChanges();

            Console.WriteLine(
                $"Игра '{title}' успешно добавлена в библиотеку.");
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine(
                $"Ошибка при сохранении в базу данных: {ex.Message}");
        }
    }

    public void ListGamesWithStudios()
    {
        try
        {
            using var db = new GameStoreContext(_conn);
            
            var games = db.Games.Include(g => g.Studio).ToList();

            if (!games.Any())
            {
                Console.WriteLine("Библиотека игр пока пуста.");
                return;
            }

            Console.WriteLine("\nАктуальный список игр ");
            foreach (var g in games)
            {
                string studioName = g.Studio?.Name ?? "Неизвестная студия";
                Console.WriteLine($"{g.Title,-20} " +
                    $"| Цена: {g.Price,8:C} " +
                    $"| Студия: {studioName}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при получении" +
                $" списка игр: {ex.Message}");
        }
    }
}