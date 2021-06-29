using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscordImageDownloader.Discord.Models
{
    internal class MessageModel
    {
        public string Id { get; set; }

        public long Type { get; set; }

        [JsonProperty("channel_id")]
        public string ChannelId { get; set; }

        public AuthorModel Author { get; set; }

        public IReadOnlyCollection<AttachmentModel> Attachments { get; set; }

        public DateTimeOffset Timestamp { get; set; }
    }
}