using MySqlConnector;
using Telegram.Bot.Types;

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

        public static async Task ReadFullData()
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

        public static async Task InsertData(long userId, string personName, DateTime date)
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

        public static async Task UpdateData(long userId, string personName, DateTime date)
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


        public static async Task DeleteData(long userId, string personName)
        {
            await using var connection = new MySqlConnection(_builder.ConnectionString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM users_schedule WHERE user_telegram_id = @userId AND human_in_schedule = @personName";
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@personName", personName);

            await command.ExecuteNonQueryAsync();
        }

        public static async Task ReadUserData(long userId)
        {
            await using var connection = new MySqlConnection(_builder.ConnectionString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT user_telegram_id, human_in_schedule, bday_date FROM users_schedule WHERE user_telegram_id = @userId";
            command.Parameters.AddWithValue("@userId", userId);

            await using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    Console.WriteLine(
                        "Reading from table=({0}, {1}, {2:dd.MM.yyyy})",
                        reader.GetInt64(0),
                        reader.GetString(1),
                        reader.GetDateTime(2)
                        );
                }
            }
        }

        public static async Task<List<PersonInDataBase>> GetData(long userId)
        {
            var humanDataList = new List<PersonInDataBase>();

            await using var connection = new MySqlConnection(_builder.ConnectionString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT id, user_telegram_id, human_in_schedule, bday_date FROM users_schedule " +
                                   "WHERE user_telegram_id = @userId";
            command.Parameters.AddWithValue("@userId", userId);

            await using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var humanData = new PersonInDataBase
                    {
                        Id = reader.GetInt32(0),
                        TelegramId = reader.GetInt64(1),
                        Name = reader.GetString(2),
                        BirthdayDate = reader.GetDateTime(3)
                    };

                    humanDataList.Add(humanData);
                }
            }

            return humanDataList;
        }


        public static async Task<bool> IsUserScheduleEmpty(long userId)
        {
            await using var connection = new MySqlConnection(_builder.ConnectionString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM users_schedule WHERE user_telegram_id = @userId";
            command.Parameters.AddWithValue("@userId", userId);

            var result = await command.ExecuteScalarAsync();
            var count = Convert.ToInt32(result);

            return count == 0;
        }
    }
}
