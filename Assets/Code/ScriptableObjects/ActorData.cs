using System.Threading.Tasks;
using Code.BattleSystem;
using Code.Utility;
using UnityEngine;

namespace Code.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ActorData", menuName = "Create new Actor Data", order = 0)]
    public class ActorData : ScriptableObject
    {
        public string Name;
        public int Health;
        public bool UseURLBrain;
        public string URL;
        public BattleActionData AttackActionData;
        public BattleActionData HealActionData;
        public BattleActionData GuardActionData;

 
    }
}