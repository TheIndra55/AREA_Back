using System;
using System.Linq;
using System.Threading.Tasks;

namespace AREA_Back.Action
{
    public class ABooru : IAction
    {
        public ABooru(BooruSharp.Booru.Booru booru) : base(25f)
        {
            this.booru = booru;
            id = booru.GetImage(0).GetAwaiter().GetResult().id;
        }

        public override void InternalUpdate(Action<string, string> action)
        {
            int lastId = booru.GetImage(0).GetAwaiter().GetResult().id;
            if (lastId != id)
            {
                action("A new image was uploaded on " + booru.ToString().Split('.').Last(), booru.GetImage(0).GetAwaiter().GetResult().fileUrl.AbsoluteUri);
                id = lastId;
            }
        }

        private BooruSharp.Booru.Booru booru;
        private int id;
    }
}
