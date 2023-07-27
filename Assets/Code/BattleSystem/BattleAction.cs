using System;
using System.Threading.Tasks;

namespace Code.BattleSystem
{
    /// <summary>
    /// BattleActions are created from parameters and executed on a target from a source
    /// They are fully constructed from BattleActionParameters, and apply all values from those parameters
    ///
    /// Visual display is left up to another layet
    /// /// </summary>
    public class BattleAction : BattleActionBase
    {

        public BattleAction(BattleActionParameters parameters, IBattleActor source, IBattleActor target)
        {
            Parameters = parameters;
            Source = source;
            Target = target;
        }

        public BattleAction()
        {
            BattleActionParameters parameters = new BattleActionParameters();
            Source = null;
            Target = null;
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
                ApplyHeal(Target);
            }

            //If parameters guard, apply guard!
            if (Parameters.doesApplyGuard)
            {
                ApplyGuard(Target);
            }

            return true;
        }
        
        
    }
}