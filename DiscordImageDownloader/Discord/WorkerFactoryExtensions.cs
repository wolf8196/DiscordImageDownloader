using DiscordImageDownloader.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DiscordImageDownloader.Discord
{
    public static class WorkerFactoryExtensions
    {
        public static IServiceCollection AddDiscordWorkers(this IServiceCollection services, IConfiguration configuration)
        {
            var discordSettings = configuration.GetSection("Discord").Get<Settings>();
            services.AddSingleton(discordSettings);

            foreach (var channelInfo in discordSettings.Download.Channels)
            {
                services.AddSingleton<IHostedService>(serviceProvider =>
                {
                    var logger = serviceProvider
                        .GetRequiredService<ILogger>()
                        .ForContext("App", "Discord")
                        .ForContext("Resource", channelInfo.Name);

                    var workerSettings = serviceProvider.GetRequiredService<WorkerSettings>();

                    return new BackgroundWorker(new Worker(
                        new Client(discordSettings.Auth, channelInfo.Channel, channelInfo.NotBefore),
                        new Downloader(channelInfo.Destination, channelInfo.Mode, logger),
                        workerSettings,
                        logger));
                });
            }

            return services;
        }
    }
}