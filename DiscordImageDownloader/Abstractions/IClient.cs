using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DiscordImageDownloader.Core;

namespace DiscordImageDownloader.Abstractions
{
    internal interface IClient
    {
        Task<IReadOnlyCollection<DownloadableItem>> GetNext(CancellationToken token);

        Task Reset(CancellationToken token);
    }
}