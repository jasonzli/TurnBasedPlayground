using System;
using Code.BattleSystem;
using UnityEngine;

namespace Code.ScriptableObjects
{
    [Serializable]
    public enum BattleActionType
    {
        Attack,
        Heal,
        Guard,
        None
    }
    
    [Serializable]
    [CreateAssetMenu(fileName = "BattleActionData", menuName = "Create new BattleActionData", order = 0)]
    public class BattleActionData : ScriptableObject
    {
        public BattleActionType ActionType;
        public string ActionName;
        public int HPDamage;
        public int HealAmount;
        public bool DoesApplyGuard;
        
        
        public BattleActionParameters AsParameters()
        {
            return new BattleActionParameters()
            {
                hpDamage = HPDamage,
                healAmount = HealAmount,
                doesApplyGuard = DoesApplyGuard,
            };
        }
    }
}