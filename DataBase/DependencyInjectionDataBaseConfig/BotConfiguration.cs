using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BirthdayReminder.DataBase.DependencyInjectionDataBaseConfig
{
    public class BotConfiguration
    {
        private static readonly IConfiguration Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .Build();

        public static string? GetBotToken()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(Configuration);
            serviceCollection.AddSingleton<BotConfigurationHelpers>();

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var botConfiguration = serviceProvider.GetService<BotConfigurationHelpers>();

            return botConfiguration?.ReceiveToken();
        }

        public static string? GetConnectionString()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IConfiguration>(Configuration);
            serviceCollection.AddSingleton<BotConfigurationHelpers>();

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var botConfiguration = serviceProvider.GetService<BotConfigurationHelpers>();

            return botConfiguration?.ReceiveConnectionString();
        }

    }
}
