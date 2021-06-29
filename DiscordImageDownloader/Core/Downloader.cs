using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DiscordImageDownloader.Abstractions;
using Serilog;

namespace DiscordImageDownloader.Core
{
    internal class Downloader : IDownloader
    {
        private readonly string destination;
        private readonly DownloadMode mode;
        private readonly HttpClient client;
        private readonly ILogger logger;

        public Downloader(string destination, DownloadMode mode, ILogger logger)
        {
            this.destination = destination;
            this.mode = mode;
            this.logger = logger;

            client = new HttpClient();
        }

        public async Task<bool> Download(IReadOnlyCollection<DownloadableItem> items, CancellationToken token)
        {
            var saved = 0;
            if (!Directory.Exists(destination))
            {
                logger.Warning("Directory {Directory} does not exist. Creating.", destination);
                Directory.CreateDirectory(destination);
            }

            foreach (var item in items)
            {
                token.ThrowIfCancellationRequested();

                var validName = ReplaceInvalidChars(item.Name);

                var path = Path.Combine(destination, validName);
                if (File.Exists(path))
                {
                    switch (mode)
                    {
                        case DownloadMode.SyncToLast:
                            logger.Information("File {Name} already exists. Stop looking back and return.", item.Name);
                            return false;

                        case DownloadMode.SyncAll:
                            logger.Information("File {Name} already exists. Skipping.", item.Name);
                            continue;
                        default:
                            throw new System.ArgumentException($"Invalid DownloadMode. Mode: {mode}");
                    }
                }

                var response = await client.SendAsync(item.Request, token);
                var stream = await response.Content.ReadAsStreamAsync(token);

                using (var fileStream = File.Create(path))
                {
                    await stream.CopyToAsync(fileStream, token);
                }

                logger.Information("Saved file {Name}.", item.Name);

                ++saved;
            }

            return mode == DownloadMode.SyncAll || saved == items.Count;
        }

        private static string ReplaceInvalidChars(string filename)
        {
            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        }
    }
}