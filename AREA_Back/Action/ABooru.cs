using System;
using System.Threading.Tasks;

namespace AREA_Back.Action
{
    public class ABooru : IAction
    {
        public ABooru(BooruSharp.Booru.Booru booru) : base(25f)
        {
            this.booru = booru;
            lastUrl = GetLastLink().GetAwaiter().GetResult();
        }

        public override void InternalUpdate(Action<string, string> action)
        {
            string url = GetLastLink().GetAwaiter().GetResult();
            if (lastUrl != url)
            {
                action("A new image was uploaded on " + booru.ToString(), url);
                lastUrl = url;
            }
        }

        private async Task<string> GetLastLink()
            => (await booru.GetImage(0)).fileUrl.AbsoluteUri;

        BooruSharp.Booru.Booru booru;
        string lastUrl;
    }
}
