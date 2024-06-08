using Microsoft.Extensions.Logging;
using System.Text;

namespace BirthdayReminder.DependencyInjectionConfiguration
{
    public class SqlLoggerService(ILogger<SqlLoggerService> logger)
    {
        public void LogQuery(string query, Dictionary<string, object>? parameters = null)
        {
            var logMessage = new StringBuilder();

            if (parameters is { Count: > 0 })
            {
                logMessage.AppendLine("Executing SQL Query:").AppendLine(query).AppendLine("With Parameters:");
                foreach (var param in parameters)
                {
                    logMessage.AppendLine($"{param.Key} = {param.Value}");
                }
            }
            else
            {
                logMessage.AppendLine($"Executing SQL Query: {query}");
            }

            logger.LogInformation(logMessage.ToString());
        }

        public void LogException(long userId)
        {
            logger.LogError("Error occurred for user with ID: {UserId}", userId);
        }
    }
}
