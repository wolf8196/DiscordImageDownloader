using System.Collections.Generic;

namespace DiscordImageDownloader.Discord
{
    internal class DownloadSettings
    {
        public IReadOnlyList<ChannelInfo> Channels { get; set; }
    }
}