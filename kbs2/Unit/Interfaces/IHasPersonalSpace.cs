using System;
using kbs2.Unit.Unit;

namespace kbs2.Unit.Interfaces
{
    public interface IHasPersonalSpace
    {
        Hitbox PersonalSpace
        {
            get;
            set;
        }
    }
}
