using kbs2.Unit;

namespace kbs2.WorldEntity.Structs
{
    public struct BattleModifiers
    {
        public float AttackModifier { get; }
        public ElementType ElementType { get; }

        public BattleModifiers(float attackModifier, ElementType elementType)
        {
            AttackModifier = attackModifier;
            ElementType = elementType;
        }
    }
}