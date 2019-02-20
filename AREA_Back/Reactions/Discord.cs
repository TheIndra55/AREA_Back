using Discord;
using Discord.Webhook;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AREA_Back.Reactions
{
    public class Discord : IReaction
    {
        public Discord()
        {
            string[] webhook = File.ReadAllLines("Keys/webhook.txt");
            webhookClient = new DiscordWebhookClient(ulong.Parse(webhook[0]), webhook[1]);
            webhookClient.Log += WebhookClient_Log;
        }

        private async Task WebhookClient_Log(LogMessage arg)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Discord log: " + arg.Message);
            Console.ResetColor();
        }

        public void Callback(string text, string thumbnail)
        {
            webhookClient.SendMessageAsync("", false, new Embed[]
            {
                new EmbedBuilder()
                {
                    Title = text,
                    Color = Color.Blue,
                    ThumbnailUrl = thumbnail
                }.Build()
            });
        }

        DiscordWebhookClient webhookClient;
    }
}
