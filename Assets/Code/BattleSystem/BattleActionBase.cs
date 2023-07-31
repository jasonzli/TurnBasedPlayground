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
        protected bool ApplyHPDamage(IBattleActor target)
        {
            //If they're guarded remove their guard and leave it at that
            if (target.Guarded)
            {
                target.Guarded = false;
                return false;
            }
            target.CurrentHP = Math.Max(0, target.CurrentHP - Parameters.hpDamage);
            return true;
        }
        
        protected bool ApplyHeal(IBattleActor target)
        {
            target.CurrentHP = Math.Min(target.MaxHP, target.CurrentHP + Parameters.healAmount);
            return true;
        }
        
        protected bool ApplyGuard(IBattleActor target)
        {
            target.Guarded = Parameters.doesApplyGuard;
            return true;
        }
    }
}