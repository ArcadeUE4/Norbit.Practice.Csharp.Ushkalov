using System.Text;
using System.Text.Json;

class Program
{ 

    private static readonly HttpClient _client = new HttpClient();

    static async Task Main(string[] args)
    {
        //Поддключаем поддержку старых кодировок для работы с ЦБ
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        Console.WriteLine("Запуск клиента");

        await GetCurrencyRates("14/01/2025");

        Console.WriteLine("\n" + new string('=', 50) + "\n");

        await CreateUser("Ivanov", "3200", "30");

        Console.WriteLine("\n" + new string('=', 50) + "\n");


        await GetEmployeeList();

        Console.WriteLine("\n Готово. Нажмите любую клавишу...");
        Console.ReadKey();
    }


    static async Task GetCurrencyRates(string date)
    {
        Console.WriteLine($" Запрос курсов валют {date}");
        string url = $"https://www.cbr.ru/scripts/XML_daily.asp?date_req={date}";

        try
        {

            var response = await _client.GetAsync(url);
            var bytes = await response.Content.ReadAsByteArrayAsync();

            //Переводим данные из Windows-1251 в текст
            string content = 
                Encoding.GetEncoding("windows-1251").GetString(bytes);

            Console.WriteLine($"Статус сервера: " +
                $"{(int)response.StatusCode}" +
                $" {response.StatusCode}");

            string preview = content.Length > 100 
                ? content.Substring(0,100) : content;

            Console.WriteLine($"Данные: {preview}");
        }

        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при получении валют: {ex.Message}");
        }
    }

    static async Task CreateUser(string name, string salary, string age)
    {
        Console.WriteLine("[POST] Создание нового пользователя...");

        string url = "https://dummy.restapiexample.com/api/v1/create";

        var userData = new
        {
            name = name,
            salary = salary,
            age = age
        };

        //
        string jsonBody = JsonSerializer.Serialize(userData);

        var _httpContent = new StringContent(jsonBody, 
            Encoding.UTF8, "application/json");

        try
        {
            var response = await _client.PostAsync
                (url, _httpContent);
            string result = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Статус: " +
                $"{(int)response.StatusCode}");

            Console.WriteLine($"Ответ сервера: {result}");
        }

        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

    }

    static async Task GetEmployeeList()
    {
        Console.WriteLine("[GET] Получение списка сотрудников...");
        string url = "https://dummy.restapiexample.com/api/v1/employees";

        using var request = new HttpRequestMessage(HttpMethod.Get, url);

        //Добавляем User-Agent, чтобы избежать запрета
        request.Headers.Add("User-Agent", "PostmanRuntime/7.32.3");

        try
        {
            var response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Список получен.");
            }

            else
            {
                Console.WriteLine($"Ошибка. Код состояния " +
                    $"{(int)response.StatusCode} ");

                Console.WriteLine("Совет: Dummy API " +
                    "часто выдает 429 (Too Many Requests).");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Критическая ошибка: " +
                $"{ex.Message}");
        }
    }
}