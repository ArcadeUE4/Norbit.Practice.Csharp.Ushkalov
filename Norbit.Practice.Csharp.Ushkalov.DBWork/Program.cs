using Microsoft.Extensions.Configuration;

class Program
{
    static void Main(string[] args)
    {
        // Инициализация 
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", 
            optional: false, 
            reloadOnChange: true)
            .Build();

        string connectionString = 
            config.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException
            ("Строка подключения не найдена");

        // Инициализация репозиториев 
        var efRepo = new EfRepository(connectionString);
        var adoRepo = new AdoRepository(connectionString);

        //  Главный цикл приложения
        bool isRunning = true;
        while (isRunning)
        {
            DisplayMenu();
            var input = Console.ReadLine();

            try
            {
                switch (input)
                {
                    case "1": efRepo.ListGamesWithStudios(); break;
                    case "2":
                        Console.Write("Введите название игры: ");
                        var title = Console.ReadLine();
                        
                        efRepo.CreateGame(title ?? 
                            "Без названия", 0, Guid.Empty);

                        break;
                    case "3": adoRepo.ShowStudiosReport(); break;

                    case "4":
                        Console.Write("Введите никнейм для блокировки: ");
                        adoRepo.BanPlayerAndResetBalance
                            (Console.ReadLine() ?? "");
                        break;

                    case "0": isRunning = false; break;
                    default: Console.WriteLine("Неверная команда."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Критическая ошибка]: " +
                    $"{ex.Message}");
            }
        }
    }

    private static void DisplayMenu()
    {
        Console.WriteLine("\n GameStore Management System");
        Console.WriteLine("1. Просмотр библиотеки (EF Core)");
        Console.WriteLine("2. Добавление новой игры (EF Core)");
        Console.WriteLine("3. Отчет по студиям (ADO.NET)");
        Console.WriteLine("4. Управление игроками (ADO.NET)");
        Console.WriteLine("0. Выход");
    }
}