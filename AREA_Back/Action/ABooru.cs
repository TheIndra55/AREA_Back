using System;
using System.Linq;
using System.Threading.Tasks;

namespace AREA_Back.Action
{
    public class ABooru : IAction
    {
        public ABooru(BooruSharp.Booru.Booru booru) : base(booru.ToString().Split('.').Last(), 5f)
        {
            this.booru = booru;
            id = booru.GetImage(0).GetAwaiter().GetResult().id;
        }

        public override void InternalUpdate(Action<string, string> action)
        {
            int lastId = booru.GetImage(0).GetAwaiter().GetResult().id;
            int saveId = lastId;
            int i = 0;
            int lastLastId;
            while (true)
            {
                lastLastId = lastId;
                lastId = booru.GetImage(i).GetAwaiter().GetResult().id;
                if (lastId != id)
                {
                    if (lastId == lastLastId)
                        continue;
                    i++;
                    if (i == 20)
                        break;
                }
                else
                    break;
            }
            if (i > 0)
            {
                if (i == 20)
                    action("20+ new images were uploaded on " + booru.ToString().Split('.').Last(), booru.GetImage(0).GetAwaiter().GetResult().fileUrl.AbsoluteUri);
                else if (i > 1)
                    action(i + " new images were uploaded on " + booru.ToString().Split('.').Last(), booru.GetImage(0).GetAwaiter().GetResult().fileUrl.AbsoluteUri);
                else
                    action("An image was uploaded on " + booru.ToString().Split('.').Last(), booru.GetImage(0).GetAwaiter().GetResult().fileUrl.AbsoluteUri);
                id = saveId;
            }
        }

        private BooruSharp.Booru.Booru booru;
        private int id;
    }
}
