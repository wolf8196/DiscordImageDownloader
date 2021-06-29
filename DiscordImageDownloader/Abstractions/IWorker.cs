using System.Threading;
using System.Threading.Tasks;

namespace DiscordImageDownloader.Abstractions
{
    internal interface IWorker
    {
        Task ExecuteAsync(CancellationToken stoppingToken);
    }
}