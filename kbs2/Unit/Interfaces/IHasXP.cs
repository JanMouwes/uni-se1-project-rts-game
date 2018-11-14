using System;
namespace kbs2.Unit
{
    public interface IHasXP
    {
        void AddXP(int amount);
        void RemoveXP(int amount);
        void LvlUp();
    }
}
