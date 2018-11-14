using System;
using kbs2.Unit.Model;

namespace kbs2.Unit

{
    public interface IHasHP
    {
        void Die();
        void Delete();
        void AddHP(int amount);
        void RemoveHP(int amount);
        void TakeHit(Unit_Model sender, int dmg, ElementType type);
    }
}
