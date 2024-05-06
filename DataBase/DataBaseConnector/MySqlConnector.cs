using MySqlConnector;

namespace BirthdayReminder.DataBase.DataBaseConnector
{
    public class MySqlConnector
    {
        public static async Task ConnectDataBase()
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "localhost",
                Database = "first_schema",
                UserID = "root",
                Password = "12321"
            };

            await using var connection = new MySqlConnection(builder.ConnectionString);
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
    }
}
