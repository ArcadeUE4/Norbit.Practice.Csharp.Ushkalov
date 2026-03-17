using Microsoft.Data.SqlClient;

public class AdoRepository
{
    private readonly string _connectionString;

    public AdoRepository(string conn) => 
        _connectionString = conn;

    public void ShowStudiosReport()
    {
        // Формируем сводку для аналитики. 
        string query = @"
            SELECT s.Name, COUNT(g.GameID) AS TotalGames 
            FROM Games g
            JOIN Studios s ON g.StudioID = s.StudioID
            GROUP BY s.Name";

        try
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var cmd = new SqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            Console.WriteLine("\nОтчет по портфолио студий ");
            while (reader.Read())
            {
                // Форматируем вывод под таблицу
                Console.WriteLine($"Студия: {reader["Name"],-20}" +
                    $" | Выпущено игр: {reader["TotalGames"]}");
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"" +
                $"Не удалось загрузить отчет: {ex.Message}");
        }
    }

    public void BanPlayerAndResetBalance(string nickname)
    {
        //Аннулироваание
        string query = "UPDATE Players SET Balance = 0, " +
            "IsBanned = 1 " +
            "WHERE NickName = @nick";

        try
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@nick", nickname);

            int rowsAffected = cmd.ExecuteNonQuery();

            // Даем обратную связь пользователю утилиты
            if (rowsAffected > 0)
                Console.WriteLine($"Аккаунт '{nickname}' " +
                    $"заблокирован, баланс списан.");
            else
                Console.WriteLine($"Игрок с никнеймом " +
                    $"'{nickname}' не найден в базе.");
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Сбой при обновлении " +
                $"статуса игрока: {ex.Message}");
        }
    }
}