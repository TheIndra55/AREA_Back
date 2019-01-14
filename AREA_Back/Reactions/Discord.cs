using Discord;
using Discord.Webhook;
using System.IO;

namespace AREA_Back.Reactions
{
    public class Discord : IReaction
    {
        public Discord()
        {
            string[] webhook = File.ReadAllLines("Keys/webhook.txt");
            webhookClient = new DiscordWebhookClient(ulong.Parse(webhook[0]), webhook[1]);
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
