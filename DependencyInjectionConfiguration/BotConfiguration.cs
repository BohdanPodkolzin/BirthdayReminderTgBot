using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BirthdayReminder.DependencyInjectionConfiguration;

public class BotConfiguration
{
    private static readonly ServiceProvider? ServiceProvider;

    private static readonly IConfiguration Configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", false, true)
        .Build();

    static BotConfiguration()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(Configuration);
        serviceCollection.AddSingleton<BotConfigurationHelpers>();

        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public static string? GetBotToken()
        => GetBotConfigurationHelpers().ReceiveToken();

    public static string? GetConnectionString() 
        => GetBotConfigurationHelpers().ReceiveConnectionString();


    private static BotConfigurationHelpers GetBotConfigurationHelpers()
        => ServiceProvider!.GetService<BotConfigurationHelpers>()
           ?? throw new Exception("Bot configuration helpers not found");
}