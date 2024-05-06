using System.Diagnostics.Contracts;
using MySqlConnector;

namespace BirthdayReminder.DataBase.DataBaseConnector
{
    public class MySqlConnector
    {
        private static readonly MySqlConnectionStringBuilder _builder = new()
        {
            Server = "localhost",
            Database = "first_schema",
            UserID = "root",
            Password = "12321"
        };

        

        

        public static async Task ReadFullDataFromDataBase()
        {
            await using var connection = new MySqlConnection(_builder.ConnectionString);
            Console.WriteLine("Opening Connection");
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM users_schedule";

            await using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    Console.WriteLine(
                        "Reading from table=({0}, {1}, {2}, {3:dd.MM.yyyy})",
                        reader.GetInt32(0),
                        reader.GetInt64(1),
                        reader.GetString(2),
                        reader.GetDateTime(3)
                    );
                }
            }

            Console.WriteLine("Closing connection");
        }

        public static async Task InsertDataToDataBase(long userId, string personName, DateTime date)
        {
            await using var connection = new MySqlConnection(_builder.ConnectionString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO users_schedule (user_telegram_id, human_in_schedule, bday_date) VALUES (@userId, @personName, @date)";
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@personName", personName);
            command.Parameters.AddWithValue("@date", date);

            await command.ExecuteNonQueryAsync();
        }

        public static async Task UpdateDataInDataBase(long userId, string personName, DateTime date)
        {
            await using var connection = new MySqlConnection(_builder.ConnectionString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = "UPDATE users_schedule SET bday_date = @date WHERE user_telegram_id = @userId AND human_in_schedule = @personName";
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@personName", personName);
            command.Parameters.AddWithValue("@date", date);

            await command.ExecuteNonQueryAsync();
        }
    }
}
