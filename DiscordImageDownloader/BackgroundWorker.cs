using System.Threading;
using System.Threading.Tasks;
using DiscordImageDownloader.Abstractions;
using Microsoft.Extensions.Hosting;

namespace DiscordImageDownloader
{
    internal class BackgroundWorker : BackgroundService
    {
        private readonly IWorker worker;

        public BackgroundWorker(IWorker worker)
        {
            this.worker = worker;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return worker.ExecuteAsync(stoppingToken);
        }
    }
}