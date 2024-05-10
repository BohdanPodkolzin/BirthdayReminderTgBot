using Microsoft.Extensions.Configuration;

namespace BirthdayReminder.DataBase.DependencyInjectionDataBaseConfig
{
    public class BotConfigurationHelpers(IConfiguration configuration)
    {
        public string? ReceiveToken() 
            => configuration.GetSection("BotConfigurationSettings:BotToken").Value;

        public string? ReceiveConnectionString()
            => configuration.GetConnectionString("DefaultConnection");
    }
}
