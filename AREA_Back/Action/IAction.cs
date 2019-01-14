using System;

namespace AREA_Back.Action
{
    public interface IAction
    {
        void Update(Action<string, string> action);
    }

    public static class Constants
    {
        public const float timeRef = 15f;
    }
}
