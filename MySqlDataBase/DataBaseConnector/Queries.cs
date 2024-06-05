using BirthdayReminder.DependencyInjectionConfiguration;
using BirthdayReminder.MySqlDataBase.DataBaseConnector;
using MySqlConnector;


namespace BirthdayReminder.DataBase.DataBaseConnector
{
    public enum Input
    {
        UserId = 1,
        UserIdName,
        UserIdDate,
        UserIdNameDate
    }
    
    public static class Queries
    {
        private static readonly string? ConnectionString = BotConfiguration.GetConnectionString();

        public static async Task ReadAllRecords()
        {
            await using var connection = await GetOpenConnectionAsync();
            await using var command = await GetCommandAsync(connection, "SELECT * FROM users_schedule");

            await using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    Console.WriteLine(
                        "Reading from table=({0}, {1}, {2}, {3:dd.MM.yyyy})",
                        reader.GetInt32(0),
                        reader.GetInt64(1),
                        reader.GetString(2),
                        reader.IsDBNull(3)
                            ? DateTime.MinValue
                            : reader.GetDateTime(3)
                    );
                }
            }
        }

        public static async Task InsertRecordByNameAndDate(long userId, string personName, DateTime date)
        {
            await using var connection = await GetOpenConnectionAsync();
            await using var command = await GetCommandAsync(connection,
                "INSERT INTO users_schedule (user_telegram_id, human_in_schedule, bday_date) VALUES (@userId, @personName, @date)");
            
            AddParametersByInput(command, Input.UserIdNameDate, userId, personName, date);
            await command.ExecuteNonQueryAsync();
        }

        public static async Task UpdateRecordByNameAndDate(long userId, string personName, DateTime date)
        {
            await using var connection = await GetOpenConnectionAsync();
            await using var command = await GetCommandAsync(connection,
                "UPDATE users_schedule SET bday_date = @date " +
                "WHERE user_telegram_id = @userId AND human_in_schedule = @personName");
            
            AddParametersByInput(command, Input.UserIdNameDate, userId, personName, date);
            await command.ExecuteNonQueryAsync();
        }

        public static async Task RemoveRecordByName(long userId, string? personName)
        {
            await using var connection = await GetOpenConnectionAsync();
            await using var command = await GetCommandAsync(connection,
                "DELETE FROM users_schedule WHERE user_telegram_id = @userId AND human_in_schedule = @personName");

            AddParametersByInput(command, Input.UserIdName, userId, personName);
            await command.ExecuteNonQueryAsync();
        }

        public static async Task<bool> IsRecordExist(long userId, string? personName)
        {
            await using var connection = await GetOpenConnectionAsync();
            await using var command = await GetCommandAsync(connection,
                "SELECT COUNT(*) FROM users_schedule WHERE user_telegram_id = @userId AND human_in_schedule");

            AddParametersByInput(command, Input.UserIdName, userId, personName);
            var result = await command.ExecuteScalarAsync();
            var count = Convert.ToInt32(result);

            return count != 0;
        }

        public static async Task<List<PersonInDataBase>> GetData(long userId)
        {
            await using var connection = await GetOpenConnectionAsync();
            await using var command = await GetCommandAsync(connection,
                "SELECT id, user_telegram_id, human_in_schedule, bday_date FROM users_schedule " +
                "WHERE user_telegram_id = @userId");
            
            AddParametersByInput(command, Input.UserId, userId);
            var dataset = new List<PersonInDataBase>();
            await using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var recordObj = new PersonInDataBase
                    {
                        Id = reader.GetInt32(0),
                        TelegramId = reader.GetInt64(1),
                        Name = reader.GetString(2),
                        BirthdayDate = reader.IsDBNull(3) 
                            ? DateTime.MinValue 
                            : reader.GetDateTime(3)
                    };

                    dataset.Add(recordObj);
                }
            }

            return dataset;
        }

        public static async Task RemoveAllRecords(long userId)
        {
            await using var connection = await GetOpenConnectionAsync();
            await using var command = await GetCommandAsync(connection,
                "DELETE FROM users_schedule WHERE user_telegram_id = @userId");

            AddParametersByInput(command, Input.UserId, userId);
            await command.ExecuteNonQueryAsync();
        }

        public static async Task RemoveInvalidRecords()
        {
            await using var connection = await GetOpenConnectionAsync();
            await using var command = await GetCommandAsync(connection, 
                                  "DELETE FROM users_schedule " +
                                  "WHERE user_telegram_id = 0 " +
                                  "OR human_in_schedule = 'unknown' " +
                                  "OR bday_date = '0001-01-01';");

            await command.ExecuteNonQueryAsync();
        }

        public static async Task<bool> IsUserScheduleEmpty(long userId)
        {
            await using var connection = await GetOpenConnectionAsync();
            await using var command = await GetCommandAsync(connection,
                "SELECT COUNT(*) FROM users_schedule WHERE user_telegram_id = @userId");

            AddParametersByInput(command, Input.UserId, userId);
            var result = await command.ExecuteScalarAsync();
            var count = Convert.ToInt32(result);

            return count == 0;
        }

        private static async Task<MySqlConnection> GetOpenConnectionAsync()
        {
            var connection = new MySqlConnection(ConnectionString);
            await connection.OpenAsync();

            return connection;
        }

        private static async Task<MySqlCommand> GetCommandAsync(MySqlConnection connection, string commandText)
        {
            var command = connection.CreateCommand();
            command.CommandText = commandText;

            return await Task.FromResult(command);
        }

        public static void AddParametersByInput(MySqlCommand command, Input input, long userId, string? personName = null, DateTime? date = null)
        {
            switch (input)
            {
                case Input.UserId:
                    command.Parameters.AddWithValue("@userId", userId);
                    break;
                case Input.UserIdName:
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@personName", personName);
                    break;
                case Input.UserIdDate:
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@date", date);
                    break;
                case Input.UserIdNameDate:
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@personName", personName);
                    command.Parameters.AddWithValue("@date", date);
                    break;
                default:
                    throw new ArgumentException("Invalid input for adding parameters");
            }
        }
    }
}
