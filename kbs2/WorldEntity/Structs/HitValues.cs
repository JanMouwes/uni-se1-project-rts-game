namespace kbs2.WorldEntity.Structs
{
    public struct HitValues
    {
        public int Damage { get; }

        public BattleModifiers BattleModifiers { get; }

        public HitValues(int damage, BattleModifiers battleModifiers)
        {
            Damage = damage;
            BattleModifiers = battleModifiers;
        }
    }
}