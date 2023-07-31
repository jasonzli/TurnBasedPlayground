using System;

namespace Code.BattleSystem
{   
    /// <summary>
    /// Represents a single action in a battle. This serves are the full base for what we expect
    /// an action to do to a target from a source
    /// </summary>
    [Serializable]
    public struct BattleActionParameters
    {
        public string moveName;
        public int hpDamage;
        public int healAmount;
        public bool doesApplyGuard;
        public BattleActionType battleActionType;
    
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
        
        public BattleActionType BattleActionType
        {
            get => battleActionType;
            set => battleActionType = value;
        }
        public BattleActionParameters(string moveName, int damage, int healAmount, bool applyGuard, BattleActionType actionType) : this()
        {
            MoveName = moveName;
            Damage = damage;
            HealAmount = healAmount;
            ApplyGuard = applyGuard;
            BattleActionType = actionType;
        }
    }

    public enum BattleActionType
    {
        None,
        Attack,
        Heal,
        Guard
    }
}