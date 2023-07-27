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
    /// Not used, was originally intended to be used for a remote action, but replaced with the Conductor getting actions on behalf of the enemy
    /// Because we need to know the action to trigger any other views to it
    /// </summary>
    public class URLBattleAction : BattleActionBase
    {
        private string BrainURL { get; set; }
        public URLBattleAction(string url, IBattleActor source, IBattleActor target)
        {
            BrainURL = url;
            Source = source;
            Target = target;
        }

        public URLBattleAction()
        {
            BrainURL = null;
            Source = null;
            Target = null;
        }

        public override bool Execute()
        {
            return false;
        }
        
        public async Task<bool> Execute(bool dont)
        {
            string response = await URLUtility.FetchJSONStringFromURL(BrainURL);

            BattleActionParameters battleActionData;
            if (response == null)
            {
                return false;
            }

            try
            {
                battleActionData = JsonUtility.FromJson<BattleActionParameters>(response);
            }
            catch(Exception e)
            {
                Debug.Log(e.Message);
                return false;
            }

            //Data is valid and good. Now we can execute the action
            
            //if Parameters deal damage, apply damage!
            if (battleActionData.hpDamage > 0)
            {
                ApplyHPDamage(Target);
            }

            //If battleActionData heal, apply health!
            if (battleActionData.healAmount > 0)
            {
                ApplyHeal(Source);
            }

            //If parameters guard, apply guard!
            if (battleActionData.doesApplyGuard)
            {
                ApplyGuard(Source);
            }

            return true;
        }
        
    }
}