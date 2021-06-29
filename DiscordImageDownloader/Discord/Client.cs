using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DiscordImageDownloader.Abstractions;
using DiscordImageDownloader.Core;
using DiscordImageDownloader.Discord.Models;
using Newtonsoft.Json;

namespace DiscordImageDownloader.Discord
{
    internal class Client : IClient
    {
        private const string InitUrl = "/messages?limit=50";
        private const string NextUrlTemplate = "/messages?before={0}&limit=50";

        private readonly HttpClient client;
        private readonly string url;
        private readonly DateTime notBefore;
        private MessageModel lastMessage;

        public Client(AuthSettings auth, string channel, DateTime notBefore)
        {
            client = new HttpClient();

            client.DefaultRequestHeaders.Add("authority", "discord.com");
            client.DefaultRequestHeaders.Add("sec-ch-ua", "\" Not A;Brand\";v=\"99\", \"Chromium\";v=\"90\", \"Google Chrome\";v=\"90\"");
            client.DefaultRequestHeaders.Add("authorization", auth.Token);
            client.DefaultRequestHeaders.Add("accept-language", "uk");
            client.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            client.DefaultRequestHeaders.Add("accept", "*/*");
            client.DefaultRequestHeaders.Add("sec-fetch-site", "same-origin");
            client.DefaultRequestHeaders.Add("sec-fetch-mode", "cors");
            client.DefaultRequestHeaders.Add("sec-fetch-dest", "empty");
            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.212 Safari/537.36");

            url = $"https://discord.com/api/v9/channels/{channel}";

            this.notBefore = notBefore;
        }

        public async Task<IReadOnlyCollection<DownloadableItem>> GetNext(CancellationToken token)
        {
            HttpRequestMessage request;
            if (lastMessage == null)
            {
                request = GetLatestMessagesRequest();
            }
            else
            {
                request = GetOlderMessagesRequest(lastMessage);
            }

            var response = await client.SendAsync(request, token);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Something went wrong. Status code: {response.StatusCode}, Reason: {response.ReasonPhrase}.");
            }

            var content = await response.Content.ReadAsStringAsync(token);

            var messages = JsonConvert.DeserializeObject<IReadOnlyCollection<MessageModel>>(content);

            messages = messages.Where(x => x.Timestamp >= notBefore).ToList();

            lastMessage = messages.Any() ? messages.Last() : lastMessage;

            return messages
                .SelectMany(message =>
                {
                    var result = new List<DownloadableItem>();
                    var counter = 0;
                    foreach (var attachment in message.Attachments)
                    {
                        var name = $"{message.Timestamp:yyyy-MM-ddTHH-mm-ss}_{counter}_{attachment.Id}_{message.Author.Username}_{attachment.Filename}";
                        ++counter;

                        result.Add(new DownloadableItem
                        {
                            Name = name,
                            Request = new HttpRequestMessage(HttpMethod.Get, attachment.Url)
                        });
                    }

                    return result;
                })
                .ToList();
        }

        public Task Reset(CancellationToken token)
        {
            lastMessage = null;
            return Task.CompletedTask;
        }

        private HttpRequestMessage GetLatestMessagesRequest()
        {
            return new HttpRequestMessage(HttpMethod.Get, Flurl.Url.Combine(url, InitUrl));
        }

        private HttpRequestMessage GetOlderMessagesRequest(MessageModel beforeMessage)
        {
            return new HttpRequestMessage(HttpMethod.Get, Flurl.Url.Combine(url, string.Format(NextUrlTemplate, beforeMessage.Id)));
        }
    }
}