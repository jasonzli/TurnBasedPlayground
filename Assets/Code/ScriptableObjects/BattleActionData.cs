using System;
using UnityEngine;

namespace Code.ScriptableObjects
{
    /// <summary>
    /// Battle Action Data for the creation of menus, players carry this around
    /// </summary>
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