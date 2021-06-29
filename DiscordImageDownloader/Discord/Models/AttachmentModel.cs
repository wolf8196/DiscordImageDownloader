using System;

namespace DiscordImageDownloader.Discord.Models
{
    internal class AttachmentModel
    {
        public string Id { get; set; }

        public string Filename { get; set; }

        public Uri Url { get; set; }
    }
}