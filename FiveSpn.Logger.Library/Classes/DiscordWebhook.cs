﻿using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FiveSpn.Logger.Library.Classes
{
    [JsonObject]
    public class DiscordWebhook
    {
        private readonly HttpClient _httpClient;
        public readonly string _webhookUrl;

        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }
        // ReSharper disable once InconsistentNaming
        [JsonProperty("tts")]
        public bool IsTTS { get; set; }
        [JsonProperty("embeds")]
        public List<DiscordEmbed> Embeds { get; set; } = new List<DiscordEmbed>();

        public DiscordWebhook(string webhookUrl)
        {
            _httpClient = new HttpClient();
            _webhookUrl = webhookUrl;
        }

        public DiscordWebhook(ulong id, string token) : this($"https://discordapp.com/api/webhooks/{id}/{token}")
        {
        }

        public async Task<string> Send()
        {
            var content = JsonConvert.SerializeObject(this);
            return await HttpRequests.UploadString(_webhookUrl, content);
        }

        // ReSharper disable once InconsistentNaming
        public async Task<string> Send(string content, string username = null, string avatarUrl = null, bool isTTS = false, IEnumerable<DiscordEmbed> embeds = null)
        {
            Content = content;
            Username = username;
            AvatarUrl = avatarUrl;
            IsTTS = isTTS;
            Embeds.Clear();
            if (embeds != null)
            {
                Embeds.AddRange(embeds);
            }

            return await Send();
        }
    }
}
