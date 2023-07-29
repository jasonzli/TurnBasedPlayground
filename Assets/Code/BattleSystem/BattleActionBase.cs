using System;

namespace Code.BattleSystem
{
    /// <summary>
    /// Base class with essential battle functions
    /// Might be other types of battle actions so we'll leave this for now
    /// </summary>
    public abstract class BattleActionBase : IBattleAction
    {
        public BattleActionParameters Parameters { get; protected set; }
        public IBattleActor Source { get; protected set; }
        public IBattleActor Target { get; protected set; }

        //This bool type will come in handy later
        public abstract bool Execute();
        
        // Some "essential" functions that should be consistent across any kind of battle action
        protected void ApplyHPDamage(IBattleActor target)
        {
            //If they're guarded remove their guard and leave it at that
            if (target.Guarded)
            {
                target.Guarded = false;
                return;
            }
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