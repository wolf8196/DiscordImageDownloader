using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DiscordImageDownloader.Abstractions;
using Serilog;

namespace DiscordImageDownloader.Core
{
    internal class Worker : IWorker
    {
        private readonly IClient client;
        private readonly IDownloader downloader;
        private readonly WorkerSettings settings;
        private readonly ILogger logger;

        public Worker(IClient client, IDownloader downloader, WorkerSettings settings, ILogger logger)
        {
            this.client = client;
            this.downloader = downloader;
            this.settings = settings;
            this.logger = logger;
        }

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Process(stoppingToken);

                    await client.Reset(stoppingToken);

                    logger.Information("Finished downloading latest files.");
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Unhandled exception occured. Will sleep and try next time.");
                    return;
                }

                logger.Information("Sleeping for {WaitTime}", settings.CheckInterval);

                await Task.Delay(settings.CheckInterval, stoppingToken);
            }
        }

        private async Task Process(CancellationToken token)
        {
            while (true)
            {
                var next = await client.GetNext(token);

                if (!next.Any())
                {
                    return;
                }
                else
                {
                    var proceed = await downloader.Download(next, token);

                    if (!proceed)
                    {
                        return;
                    }
                }
            }
        }
    }
}