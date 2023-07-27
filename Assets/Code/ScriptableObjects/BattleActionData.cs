using System;
using Code.BattleSystem;
using UnityEngine;

namespace Code.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(fileName = "BattleActionData", menuName = "Create new BattleActionData", order = 0)]
    public class BattleActionData : ScriptableObject
    {
        public string ActionName;
        public int HPDamage;
        public int HealAmount;
        public bool DoesApplyGuard;
        public Sprite Icon;
    }
}