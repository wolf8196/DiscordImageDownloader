using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DiscordImageDownloader.Core;

namespace DiscordImageDownloader.Abstractions
{
    internal interface IDownloader
    {
        Task<bool> Download(IReadOnlyCollection<DownloadableItem> items, CancellationToken token);
    }
}