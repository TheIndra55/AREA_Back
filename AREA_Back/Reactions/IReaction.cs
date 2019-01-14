namespace AREA_Back.Reactions
{
    public interface IReaction
    {
        void Callback(string text, string thumbnail);
    }
}
