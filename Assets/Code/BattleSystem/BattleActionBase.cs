using System;
using System.Threading.Tasks;

namespace Code.BattleSystem
{
    public abstract class BattleActionBase : IBattleAction
    {
        public BattleActionParameters Parameters { get; protected set; }
        public IBattleActor Source { get; protected set; }
        public IBattleActor Target { get; protected set; }

        //This bool type will come in handy later
        public abstract Task<bool> Execute();
        
        protected void ApplyHPDamage(IBattleActor target)
        {
            target.CurrentHP = Math.Max(0, target.CurrentHP - Parameters.hpDamage);
        }
        
        protected void ApplyHeal(IBattleActor target)
        {
            target.CurrentHP = Math.Min(target.MaxHP, target.CurrentHP + Parameters.healAmount);
        }
        
        protected void ApplyGuard(IBattleActor target)
        {
            target.Guarded = Parameters.doesApplyGuard;
        }
    }
}