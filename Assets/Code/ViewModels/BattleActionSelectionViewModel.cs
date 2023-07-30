using System;
using System.Collections.Generic;
using Code.BattleSystem;
using Code.ProtoVM;
using Code.ScriptableObjects;
using Code.Utility;

namespace Code.ViewModels
{
    /// <summary>
    /// A viewmodel for the selection panel
    /// There is potentially a universe where an action can be performed on *different* targets, and so the Update
    /// Target function is there.
    ///
    /// This is where unsafe battle actions are made and sent around.
    /// </summary>
    public class BattleActionSelectionViewModel : IViewModel
    {
        public List<BattleActionData> AvailableBattleActionData { get; private set; }
        private IBattleActor Source;
        private IBattleActor Target;
        public Observable<bool> UnsafeBattle = new Observable<bool>();
        public Observable<bool> Visibility = new Observable<bool>();

        public BattleActionSelectionViewModel(List<BattleActionData> availableBattleActionData, IBattleActor source, IBattleActor target, bool unsafeBattle = false)
        {
            AvailableBattleActionData = availableBattleActionData;
            Source = source;
            Target = target;
            UnsafeBattle.Value = unsafeBattle;
        }
        
        public Action<IBattleAction> OnActionSelected;
        
        /// <summary>
        /// This is the function that sends to the battle system that an action has been selected
        /// Creates the battle action to execute from the parameters
        /// </summary>
        /// <param name="actionData">The set action data from the action panel</param>
        /// <remarks> Imagine a world where parameters can be modified and updated, that's why we're creating them here</remarks>
        public void SendActionToConductor(BattleActionData actionData)
        {
            if (UnsafeBattle)
            {
                SendUnsafeBattleActionData(actionData);
            }
            else
            {
                SendBattleActionData(actionData);
            }
        }
        public void SendBattleActionData(BattleActionData actionData)
        {
            BattleAction newAction = new BattleAction (actionData.AsSafeBattleActionParameters() , Source, Target);
            OnActionSelected?.Invoke(newAction);
        }
        
        public void SendUnsafeBattleActionData(BattleActionData actionData)
        {
            BattleAction newAction = new BattleAction (actionData.AsBattleActionParameters() , Source, Target);
            UnsafeBattleAction unsafeBattleAction = new UnsafeBattleAction(newAction);
            OnActionSelected?.Invoke(unsafeBattleAction);
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