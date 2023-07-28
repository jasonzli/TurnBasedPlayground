using Code.BattleSystem;
using Code.ScriptableObjects;

namespace Code.Utility
{
    /// <summary>
    /// Extensions for the battleactionparameters struct
    /// </summary>
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
                MoveName = data.ActionName,
                hpDamage = data.HPDamage,
                healAmount = data.HealAmount,
                doesApplyGuard = data.DoesApplyGuard,
            };

        }
    }
}