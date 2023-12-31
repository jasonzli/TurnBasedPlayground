using System;
using System.Threading.Tasks;

namespace Code.BattleSystem
{
    /// <summary>
    /// BattleActions are created from parameters and executed on a target from a source
    /// They are fully constructed from BattleActionParameters, and apply all values from those parameters
    /// using the base's constraints for how to apply damage, healing, and guard
    /// /// </summary>
    public class BattleAction : BattleActionBase
    {

        public BattleAction(BattleActionParameters parameters, IBattleActor source, IBattleActor target)
        {
            Parameters = parameters;
            Source = source;
            Target = target;
        }
        
        public override bool Execute()
        {
            //if Parameters deal damage, apply damage!
            if (Parameters.hpDamage > 0)
            {
                ApplyHPDamage(Target);
            }
            
            //If Parameters heal, apply health!
            if (Parameters.healAmount > 0)
            {
                ApplyHeal(Source);
            }

            //If parameters guard, apply guard!
            if (Parameters.doesApplyGuard)
            {
                ApplyGuard(Source);
            }

            //if target is still guarded, remove the guard
            if (Target.Guarded)
            {
                Target.Guarded = false;
            }
            return true;
        }


    }
}