using Microsoft.Extensions.Logging;

namespace BirthdayReminder.DependencyInjectionConfiguration
{
    public class SqlLoggerService(ILogger<SqlLoggerService> logger)
    {
        public void LogQuery(string query, Dictionary<string, object>? parameters = null)
        {
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    logger.LogInformation("Parameter: {Key} = {Value}", param.Key, param.Value);
                }
            }
            logger.LogInformation("Executing SQL Query: {Query}", query);
        }
    }
}