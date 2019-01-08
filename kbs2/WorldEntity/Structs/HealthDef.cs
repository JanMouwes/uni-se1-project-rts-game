namespace kbs2.WorldEntity.Structs
{
    public struct HealthDef
    {
        public HealthDef(int maxHealth)
        {
            MaxHealth = maxHealth;
        }

        public int MaxHealth { get; }
    }
}