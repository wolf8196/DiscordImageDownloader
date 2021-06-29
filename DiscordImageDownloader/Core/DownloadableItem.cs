using System.Net.Http;

namespace DiscordImageDownloader.Core
{
    internal class DownloadableItem
    {
        public string Name { get; set; }

        public HttpRequestMessage Request { get; set; }
    }
}