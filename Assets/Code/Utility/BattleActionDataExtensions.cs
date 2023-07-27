using Code.BattleSystem;
using Code.ScriptableObjects;

namespace Code.Utility
{
    public static class BattleActionDataExtensions
    {
        public static BattleAction AsBattleAction(this BattleActionData data, IBattleActor source, IBattleActor target)
        {
            return new BattleAction(data.AsBattleActionParameters(), source, target);
        }

        public static BattleActionParameters AsBattleActionParameters(this BattleActionData data)
        {
        
            return new BattleActionParameters()
            {
                hpDamage = data.HPDamage,
                healAmount = data.HealAmount,
                doesApplyGuard = data.DoesApplyGuard,
            };

        }
    }
}