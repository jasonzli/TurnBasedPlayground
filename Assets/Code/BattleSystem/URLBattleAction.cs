using System;
using System.Threading.Tasks;
using Code.Utility;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

namespace Code.BattleSystem
{
    /// <summary>
    /// A BattleAction that is created from a URL,
    /// Was originally intended to be used for a remote action, but replaced with the Conductor getting actions on behalf of the enemy
    /// Because we need to know the action to update any viewmodels with its data to it
    /// </summary>
    public class URLBattleAction : BattleActionBase
    {
        public URLBattleAction(BattleActionParameters parameters, IBattleActor source, IBattleActor target)
        {
            Parameters = parameters;
            Source = source;
            Target = target;
        }

        public URLBattleAction()
        {
            Parameters = new BattleActionParameters();
            Source = null;
            Target = null;
        }

        public override bool Execute()
        {
            //Apply parameters intelligently
            
            //if Parameters deal damage, apply damage!
            if (Parameters.hpDamage > 0)
            {
                ApplyHPDamage(Target);
            }

            //If battleActionData heal, apply health!
            if (Parameters.healAmount > 0)
            {
                ApplyHeal(Source);
            }

            //If parameters guard, apply guard!
            if (Parameters.doesApplyGuard)
            {
                ApplyGuard(Source);
            }

            //If the target is still guarded at this time, remove the guard
            if (Target.Guarded)
            {
                Target.Guarded = false;
            }
            return true;
        }
        
    }
}