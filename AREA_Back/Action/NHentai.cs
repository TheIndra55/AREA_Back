using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AREA_Back.Action
{
    class NHentai : IAction
    {
        public NHentai(string username)
        {
            this.username = username;
            realUsername = username.Split('/').Last();
            var v = GetFirstsDoujinshiAsync().GetAwaiter().GetResult();
            d1 = v.Item1;
            d2 = v.Item2;
        }

        public void Update(Action<string, string> action)
        {
            if (DateTime.Now.Subtract(lastRequest).TotalSeconds > Constants.timeRef)
            {
                var v = GetFirstsDoujinshiAsync().GetAwaiter().GetResult();
                if ((d1 == null && v.Item1 != null)
                    || (d1.Value == v.Item2))
                {
                    action(realUsername + " added a doujinshi to their favorite: " + v.Item1.Value.Name, v.Item1.Value.Url);
                    d1 = v.Item1;
                    d2 = v.Item2;
                }
                else if ((d1 != null && v.Item1 == null)
                    || (d2.Value == v.Item1))
                {
                    action(realUsername + " removed a doujinshi from their favorite.", null);
                    d1 = v.Item1;
                    d2 = v.Item2;
                }
                lastRequest = DateTime.Now;
            }
        }

        private async Task<Tuple<Doujinshi?, Doujinshi?>> GetFirstsDoujinshiAsync()
        {
            string html;
            using (HttpClient hc = new HttpClient())
                html = await hc.GetStringAsync("https://nhentai.net/users/" + username);
            string[] doujins = html.Split(new string[] { "<div class=\"gallery\"" }, StringSplitOptions.None);
            if (doujins.Length == 1)
                return (new Tuple<Doujinshi?, Doujinshi?>(null, null));
            if (doujins.Length == 2)
                return (new Tuple<Doujinshi?, Doujinshi?>(ParseDoujinshi(doujins[1]), null));
            return (new Tuple<Doujinshi?, Doujinshi?>(ParseDoujinshi(doujins[1]), ParseDoujinshi(doujins[2])));
        }

        private Doujinshi ParseDoujinshi(string html)
        {
            var match = Regex.Match(html, "src=\"([^\"]+)\"( width=\"[0-9]+\" height=\"[0-9]+\" \\/><\\/noscript>)?( \\/>)?<div class=\"caption\">([^<]+)").Groups;
            return (new Doujinshi()
            {
                Name = match[4].Value,
                Url = (match[1].Value.StartsWith("//") ? "http:" : "") + match[1].Value
            });
        }

        private DateTime lastRequest;
        private Doujinshi? d1;
        private Doujinshi? d2;
        private string username;
        private string realUsername;

        private struct Doujinshi
        {
            public string Name;
            public string Url;

            public override bool Equals(object obj)
            {
                if (!(obj is Doujinshi))
                    return false;

                var doujinshi = (Doujinshi)obj;
                return Name == doujinshi.Name;
            }

            public override int GetHashCode()
            {
                var hashCode = -1254404684;
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Url);
                return hashCode;
            }

            public static bool operator ==(Doujinshi d1, Doujinshi d2)
                => d1.Name == d2.Name;

            public static bool operator !=(Doujinshi d1, Doujinshi d2)
                => !(d1 == d2);
        }
    }
}
