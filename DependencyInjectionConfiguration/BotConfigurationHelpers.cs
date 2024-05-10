using Microsoft.Extensions.Configuration;

namespace BirthdayReminder.DependencyInjectionConfiguration;

public class BotConfigurationHelpers(IConfiguration configuration)
{
    public string? ReceiveToken()
        => configuration.GetSection("BotConfigurationSettings:BotToken").Value;

    public string? ReceiveConnectionString()
        => configuration.GetSection("BotConfigurationSettings:ConnectionStrings:DefaultConnection").Value;
}