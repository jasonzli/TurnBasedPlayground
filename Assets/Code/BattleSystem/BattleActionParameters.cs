namespace Code.BattleSystem
{   /// <summary>
    /// Represents a single action in a battle.
    /// </summary>
    public struct BattleActionParameters
    {
        public string moveName;
        public int hpDamage;
        public int healAmount;
        public bool doesApplyGuard;
    
        public string MoveName
        {
            get => moveName;
            set => moveName = value;
        }
    
        public int Damage
        {
            get => hpDamage;
            set => hpDamage = value;
        }
    
        public int HealAmount
        {
            get => healAmount;
            set => healAmount = value;
        }
    
        public bool ApplyGuard
        {
            get => doesApplyGuard;
            set => doesApplyGuard = value;
        }

        public BattleActionParameters(string moveName, int damage, int healAmount, bool applyGuard) : this()
        {
            MoveName = moveName;
            Damage = damage;
            HealAmount = healAmount;
            ApplyGuard = applyGuard;
        }
    }
}