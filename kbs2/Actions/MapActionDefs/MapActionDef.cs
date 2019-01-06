using kbs2.Actions.Interfaces;

namespace kbs2.Actions.GameActionDefs
{
    public abstract class MapActionDef : IMapActionDef
    {
        public uint CooldownTime { get; }

        public string Icon { get; }


        protected MapActionDef(uint cooldown, string imageSource)
        {
            CooldownTime = cooldown;
            Icon = imageSource;
        }
    }
}