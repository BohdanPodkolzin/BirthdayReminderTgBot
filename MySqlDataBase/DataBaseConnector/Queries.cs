using BirthdayReminder.DependencyInjectionConfiguration;
using BirthdayReminder.Telegram.Models;
using MySqlConnector;
using Newtonsoft.Json.Linq;
using Telegram.Bot.Types;

namespace BirthdayReminder.MySqlDataBase.DataBaseConnector
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
        private static readonly SqlLoggerService? Logger = BotConfiguration.SqlLoggerService;

        public static async Task ReadAllRecords()
        {
            await using var connection = await GetOpenConnectionAsync();

            const string query = "SELECT * FROM users_schedule";
            await using var command = await GetCommandAsync(connection, query);

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                Console.WriteLine(
                    "Reading from table=({0}, {1}, {2:dd.MM.yyyy})",
                    reader.GetInt64(0),
                    reader.GetString(1),
                    reader.IsDBNull(2)
                        ? DateTime.MinValue
                        : reader.GetDateTime(3)
                );
            }

            Logger?.LogQuery(query);
        }

        public static async Task InsertRecordByNameAndDate(long userId, string personName, DateTime date)
        {
            await using var connection = await GetOpenConnectionAsync();
            await using var command = await GetCommandAsync(connection,
                "INSERT INTO users_schedule (user_telegram_id, record, bday_date) VALUES (@userId, @personName, @date)");
            
            AddParametersByInput(command, Input.UserIdNameDate, userId, personName, date);
            await command.ExecuteNonQueryAsync();
        }

        public static async Task UpdateRecordByNameAndDate(long userId, string personName, DateTime date)
        {
            await using var connection = await GetOpenConnectionAsync();
            await using var command = await GetCommandAsync(connection,
                "UPDATE users_schedule SET bday_date = @date " +
                "WHERE user_telegram_id = @userId AND record = @personName");
            
            AddParametersByInput(command, Input.UserIdNameDate, userId, personName, date);
            await command.ExecuteNonQueryAsync();
        }

        public static async Task RemoveRecordByName(long userId, string? personName)
        {
            await using var connection = await GetOpenConnectionAsync();
            await using var command = await GetCommandAsync(connection,
                "DELETE FROM users_schedule WHERE user_telegram_id = @userId AND record = @personName");

            AddParametersByInput(command, Input.UserIdName, userId, personName);
            await command.ExecuteNonQueryAsync();
        }

        public static async Task<bool> IsRecordExist(long userId, string? personName)
        {
            await using var connection = await GetOpenConnectionAsync();
            await using var command = await GetCommandAsync(connection,
                "SELECT COUNT(*) FROM users_schedule WHERE user_telegram_id = @userId AND record = @personName");

            AddParametersByInput(command, Input.UserIdName, userId, personName);
            var result = await command.ExecuteScalarAsync();
            var count = Convert.ToInt32(result);

            return count != 0;
        }

        public static async Task<List<RecordsBirthdayListModel>> GetRecordsData(long userId)
        {
            await using var connection = await GetOpenConnectionAsync();
            await using var command = await GetCommandAsync(connection,
                "SELECT user_telegram_id, record, bday_date FROM users_schedule " +
                "WHERE user_telegram_id = @userId");
            
            AddParametersByInput(command, Input.UserId, userId);
            var dataset = new List<RecordsBirthdayListModel>();

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var recordObj = new RecordsBirthdayListModel
                {
                    TelegramId = reader.GetInt64(0),
                    Name = reader.GetString(1),
                    BirthdayDate = reader.IsDBNull(2) 
                        ? DateTime.MinValue 
                        : reader.GetDateTime(2)
                };

                dataset.Add(recordObj);
            }

            return dataset;
        }

        public static async Task<List<RecordsCoordinatesModel>> GetRecordsTimezone(long userId)
        {

            await using var connection = await GetOpenConnectionAsync();
            await using var command = await GetCommandAsync(connection,
                "SELECT telegram_id, latitude, longitude FROM users_place_coords" +
                "WHERE telegram_id = @userId");
            command.Parameters.AddWithValue("@userId", userId);


            var recordCoordinatesList = new List<RecordsCoordinatesModel>();
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var coordinateObject = new RecordsCoordinatesModel
                {
                    TelegramId = reader.GetInt64(0),
                    Latitude = reader.GetString(1),
                    Longitude = reader.GetString(2)
                };
                recordCoordinatesList.Add(coordinateObject);
            }

            return recordCoordinatesList;
        }

        public static async Task<List<long>> GetUsersIds()
        {
            await using var connection = await GetOpenConnectionAsync();
            await using var command = await GetCommandAsync(connection,
                "SELECT DISTINCT user_telegram_id FROM users_schedule");

            var dataset = new List<long>();

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                dataset.Add(reader.GetInt64(0));
            }

            return dataset;
        }

        public static async Task IncludeLatitudeAndLongitude(long userId, string latitude, string longitude)
        {
            await using var connection = await GetOpenConnectionAsync();
            string query;
            if (await IsUserExist(userId))
            {
                query =
                    "UPDATE users_place_coords SET latitude = @latitude, longitude = @longitude WHERE telegram_id = @userId;";
            }
            else
            {
                query =
                    "INSERT INTO users_place_coords (telegram_id, latitude, longitude, today_date) VALUES (@userId, @latitude, @longitude, @today_date)";
            }
            await using var command = await GetCommandAsync(connection, query);

            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@latitude", latitude);
            command.Parameters.AddWithValue("@longitude", longitude);

            if (query.Contains("INSERT"))
            {
                var todayDate = await GetDateTime(latitude, longitude);
                command.Parameters.AddWithValue("@today_date", todayDate);
                Console.WriteLine(todayDate);
            }

            await command.ExecuteNonQueryAsync();
        }
        private static HttpClient GetConfiguredHttpClient()
            => new() { DefaultRequestHeaders = { { "User-Agent", "CSharpApp" } } };

        public static async Task<DateTime> GetDateTime(string? lat, string? lon)
        {
            var url =
                $"https://api.timezonedb.com/v2.1/get-time-zone?key=7TVJUMUJ9LMG&format=json&by=position&lat={lat}&lng={lon}";
            using var client = GetConfiguredHttpClient();

            var response = await client.GetStringAsync(url);
            var results = JObject.Parse(response);

            var time = (results["formatted"]?.Value<DateTime>())
                       ?? throw new Exception("Time is null");

            return time;
        }


        public static async Task<(string?, string?)> GetLatitudeAndLongitudeFromDatabase(long userId)
        {
            await using var connection = await GetOpenConnectionAsync();
            await using var command = await GetCommandAsync(connection,
                "SELECT latitude, longitude FROM users_place_coords WHERE telegram_id = @userId");
            command.Parameters.AddWithValue("@userId", userId);

            await using var reader = await command.ExecuteReaderAsync();
            if (!await reader.ReadAsync()) return (null, null);

            var latitude = reader["latitude"] as string;
            var longitude = reader["longitude"] as string;
            return (latitude, longitude);

        }

        public static async Task UpdateLatitudeAndLongitude(long userId, string latitude, string longitude)
        {
            await using var connection = await GetOpenConnectionAsync();
            await using var command = await GetCommandAsync(connection,
                "UPDATE users_place_coords SET latitude = @latitude, longitude = @longitude WHERE telegram_id = @userId;");

            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@latitude", latitude);
            command.Parameters.AddWithValue("@longitude", longitude);
            
            await command.ExecuteNonQueryAsync();

        }

        public static async Task RemoveAllRecords(long userId)
        {
            await using var connection = await GetOpenConnectionAsync();
            await using var command = await GetCommandAsync(connection,
                "DELETE FROM users_schedule WHERE user_telegram_id = @userId");
            command.Parameters.AddWithValue("@userId", userId);
                
            await command.ExecuteNonQueryAsync();
        }

        public static async Task RemoveInvalidRecords()
        {
            await using var connection = await GetOpenConnectionAsync();
            await using var command = await GetCommandAsync(connection, 
                                  "DELETE FROM users_schedule " +
                                  "WHERE user_telegram_id = 0 " +
                                  "OR record = 'unknown' " +
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

        public static async Task<bool> IsUserExist(long userId)
        {
            await using var connection = await GetOpenConnectionAsync();
            await using var command = await GetCommandAsync(connection,
                "SELECT COUNT(*) FROM users_place_coords WHERE telegram_id = @userId");

            AddParametersByInput(command, Input.UserId, userId);
            var result = await command.ExecuteScalarAsync();
            var count = Convert.ToInt32(result);

            Console.WriteLine(count);
            return count > 0;
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
            var parameters = new Dictionary<string, object>();
            switch (input)
            {
                case Input.UserId:
                    command.Parameters.AddWithValue("@userId", userId);
                    parameters["@userId"] = userId;
                    break;
                case Input.UserIdName:
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@personName", personName);
                    parameters["@userId"] = userId;
                    parameters["@personName"] = personName ?? "unknown";
                    break;
                case Input.UserIdDate:
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@date", date);
                    parameters["@userId"] = userId;
                    parameters["@date"] = date ?? DateTime.MinValue;
                    break;
                case Input.UserIdNameDate:
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@personName", personName);
                    command.Parameters.AddWithValue("@date", date);
                    parameters["@userId"] = userId;
                    parameters["@personName"] = personName ?? "unknown";
                    parameters["@date"] = date ?? DateTime.MinValue;
                    break;
                default:
                    Logger?.LogException(userId);
                    return;
            }

            Logger?.LogQuery(command.CommandText, parameters);
        }
    }

}
