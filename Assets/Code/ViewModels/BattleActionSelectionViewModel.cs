using System;
using System.Collections.Generic;
using Code.BattleSystem;
using Code.ScriptableObjects;
using Code.Utility;

namespace Code.ViewScripts
{
    public class BattleActionSelectionViewModel
    {
        public List<BattleActionData> AvailableBattleActionData { get; private set; }
        private IBattleActor Source;
        private IBattleActor Target;

        public BattleActionSelectionViewModel(List<BattleActionData> availableBattleActionData, IBattleActor source, IBattleActor target)
        {
            AvailableBattleActionData = availableBattleActionData;
            Source = source;
            Target = target;
        }
        
        public Action<IBattleAction> OnActionSelected;
        
        public void SendBattleActionData(BattleActionData actionData)
        {
            //A hack to make sure our heals and everything work correctly
            IBattleActor tempTarget = Target;
            if (actionData.DoesApplyGuard)
            {
                tempTarget = Source;
            }

            if (actionData.HealAmount > 0)
            {
                tempTarget = Source;
            }
            BattleAction newAction = new BattleAction (actionData.AsBattleActionParameters() , Source, tempTarget);
            OnActionSelected?.Invoke(newAction);
        }

        public void UpdateTarget(IBattleActor newTarget)
        {
            Target = newTarget;
        }
    }
}