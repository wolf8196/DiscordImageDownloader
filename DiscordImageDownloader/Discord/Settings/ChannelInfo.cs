using System;
using DiscordImageDownloader.Core;

namespace DiscordImageDownloader.Discord
{
    internal class ChannelInfo
    {
        public string Name { get; set; }

        public DateTime NotBefore { get; set; }

        public DownloadMode Mode { get; set; }

        public string Channel { get; set; }

        public string Destination { get; set; }
    }
}