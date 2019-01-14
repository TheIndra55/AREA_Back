using Discord;
using Discord.Webhook;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace AREA_Back.Action
{
    public class Konachan
    {
        public Konachan(string username)
        {
            this.username = username;
            secRef = 30f;
            using (HttpClient hc = new HttpClient())
            {
                dynamic json = JsonConvert.DeserializeObject(hc.GetStringAsync("http://konachan.net/post.json?tags=vote%3A3%3A" + username).GetAwaiter().GetResult());
                lastCount = json.Count;
            }
            lastRequest = DateTime.Now;
        }

        public void Update(DiscordWebhookClient webhookClient)
        {
            if (DateTime.Now.Subtract(lastRequest).TotalSeconds > secRef)
            {
                int currCount;
                dynamic json;
                using (HttpClient hc = new HttpClient())
                {
                    json = JsonConvert.DeserializeObject(hc.GetStringAsync("http://konachan.net/post.json?tags=vote%3A3%3A" + username).GetAwaiter().GetResult());
                    currCount = json.Count;
                }
                if (currCount != lastCount)
                {
                    Console.WriteLine(json[json.Count - 1].file_url);
                    if (currCount < lastCount)
                        webhookClient.SendMessageAsync("", false, new Embed[]
                        {
                            new EmbedBuilder()
                            {
                                Title = username + " removed an image from their favorite.",
                                Color = Color.Blue
                            }.Build()
                        });
                    else
                        webhookClient.SendMessageAsync("", false, new Embed[]
                        {
                            new EmbedBuilder()
                            {
                                Title = username + " added an image to their favorite.",
                                ThumbnailUrl = json[json.Count - 1].file_url,
                                Color = Color.Blue
                            }.Build()
                        });
                    lastCount = currCount;
                }
                lastRequest = DateTime.Now;
            }
        }

        private string username;
        private int lastCount;
        private DateTime lastRequest;
        private readonly float secRef;
    }
}
