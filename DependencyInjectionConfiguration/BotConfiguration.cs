using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BirthdayReminder.DependencyInjectionConfiguration;

public class BotConfiguration
{
    public static ServiceProvider? ServiceProvider { get; private set; }

    private static readonly IConfiguration Configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

    static BotConfiguration()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(Configuration);

        serviceCollection.AddSingleton<BotConfigurationHelpers>();
        serviceCollection.AddLogging(builder => builder.AddConsole());
        serviceCollection.AddSingleton<SqlLoggerService>();

        ServiceProvider = serviceCollection.BuildServiceProvider();
        SqlLoggerService = ServiceProvider.GetRequiredService<SqlLoggerService>();
    }

    public static SqlLoggerService SqlLoggerService { get; }

    public static string? GetBotToken()
        => GetBotConfigurationHelpers().ReceiveToken();

    public static string? GetConnectionString() 
        => GetBotConfigurationHelpers().ReceiveConnectionString();

    private static BotConfigurationHelpers GetBotConfigurationHelpers()
        => ServiceProvider!.GetService<BotConfigurationHelpers>()
           ?? throw new Exception("Bot configuration helpers not found");

    //public static ILogger<BotConfiguration>? Logger { get; }
}