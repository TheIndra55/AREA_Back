using System;

namespace AREA_Back.Action
{
    public abstract class IAction
    {
        public abstract void InternalUpdate(Action<string, string> action);

        public IAction(string name, float timer)
        {
            this.timer = timer;
            isBusy = false;
            Program.AddService(name);
        }

        public void Update(Action<string, string> action)
        {
            if (DateTime.Now.Subtract(lastRequest).TotalSeconds > timer && !isBusy)
            {
                isBusy = true;
                InternalUpdate(action);
                lastRequest = DateTime.Now;
                isBusy = false;
            }
        }

        private DateTime lastRequest;
        private readonly float timer;
        private bool isBusy;
    }
}
