using kbs2.Actions.Interfaces;

namespace kbs2.Actions.GameActionDefs
{
    public abstract class GameActionDef : IGameActionDef
    {
        public uint Cooldown { get; }

        public string ImageSource { get; }


        protected GameActionDef(uint cooldown, string imageSource)
        {
            Cooldown = cooldown;
            ImageSource = imageSource;
        }
    }
}