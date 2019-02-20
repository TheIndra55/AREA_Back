using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace AREA_Back.Action
{
    public class KonachanFavorite : IAction
    {
        public KonachanFavorite(string username) : base("Konachan Favorite", 5f)
        {
            this.username = username;
            using (HttpClient hc = new HttpClient())
            {
                dynamic json = JsonConvert.DeserializeObject(hc.GetStringAsync("http://konachan.net/post.json?tags=vote%3A3%3A" + username).GetAwaiter().GetResult());
                lastCount = json.Count;
            }
        }

        public override void InternalUpdate(Action<string, string> action)
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
                if (currCount < lastCount)
                    action(username + " removed an image from their favorite.", null);
                else
                    action(username + " added an image to their favorite.", (string)json[json.Count - 1].file_url);
                lastCount = currCount;
            }
        }

        private string username;
        private int lastCount;
    }
}
