using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace AREA_Back.Action
{
    public class Konachan : IAction
    {
        public Konachan(string username)
        {
            this.username = username;
            using (HttpClient hc = new HttpClient())
            {
                dynamic json = JsonConvert.DeserializeObject(hc.GetStringAsync("http://konachan.net/post.json?tags=vote%3A3%3A" + username).GetAwaiter().GetResult());
                lastCount = json.Count;
            }
            lastRequest = DateTime.Now;
        }

        public void Update(Action<string, string> action)
        {
            if (DateTime.Now.Subtract(lastRequest).TotalSeconds > Constants.timeRef)
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
                lastRequest = DateTime.Now;
            }
        }

        private string username;
        private int lastCount;
        private DateTime lastRequest;
    }
}
