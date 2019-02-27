using System;
using System.Linq;

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
            int diff = lastId - id;
            if (diff > 0)
            {
                if (diff > 1)
                    action(diff + " new images were uploaded on " + booru.ToString().Split('.').Last(), booru.GetImage(0).GetAwaiter().GetResult().fileUrl.AbsoluteUri);
                else
                    action("An image was uploaded on " + booru.ToString().Split('.').Last(), booru.GetImage(0).GetAwaiter().GetResult().fileUrl.AbsoluteUri);
                id = lastId;
            }
        }

        private BooruSharp.Booru.Booru booru;
        private int id;
    }
}
