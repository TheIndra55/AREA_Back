using System;

namespace AREA_Back.Action
{
    public abstract class IAction
    {
        public abstract void InternalUpdate(Action<string, string> action);

        public IAction(float timer)
        {
            this.timer = timer;
        }

        public void Update(Action<string, string> action)
        {
            if (DateTime.Now.Subtract(lastRequest).TotalSeconds > timer)
            {
                InternalUpdate(action);
                lastRequest = DateTime.Now;
            }
        }

        private DateTime lastRequest;
        private float timer;
    }
}
