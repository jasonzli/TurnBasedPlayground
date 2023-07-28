using System;
using System.Collections.Generic;
using Code.BattleSystem;
using Code.ProtoVM;
using Code.ScriptableObjects;
using Code.Utility;
using Code.ViewModels;

namespace Code.ViewScripts
{
    /// <summary>
    /// A viewmodel for the selection panel
    /// There is potentially a universe where an action can be performed on *different* targets, and so the Update
    /// Target function is there.
    /// </summary>
    public class BattleActionSelectionViewModel : IViewModel
    {
        public List<BattleActionData> AvailableBattleActionData { get; private set; }
        private IBattleActor Source;
        private IBattleActor Target;
        public Observable<bool> Visibility = new Observable<bool>();

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
            //Originally we could use this to make it so we could "aim" the attacks, left for posterity
            // IBattleActor tempTarget = Target;
            // if (actionData.DoesApplyGuard)
            // {
            //     tempTarget = Source;
            // }
            //
            // if (actionData.HealAmount > 0)
            // {
            //     tempTarget = Source;
            // }
            BattleAction newAction = new BattleAction (actionData.AsBattleActionParameters() , Source, Target);
            OnActionSelected?.Invoke(newAction);
        }

        public void UpdateTarget(IBattleActor newTarget)
        {
            Target = newTarget;
        }

        public void SetVisibility(bool visibility)
        {
            Visibility.Value = visibility;
        }
    }
}