using System;
using Code.BattleSystem;
using Code.ProtoVM;
using Code.ScriptableObjects;
using Code.ViewModels;

namespace Code.DebugMenu
{
    /// <summary>
    /// The actual behaviors for the debugging menu (mostly). THere's some behaviors coded up on the higher level (like
    /// Unity screen and Quit functionality). Here is the values we're going to set and manipulate on the conductor
    /// </summary>
    public class DebugViewModel : IViewModel
    {

        public Observable<int> HealthValue = new Observable<int>();
        public Observable<int> HealPower = new Observable<int>();
        public Observable<int> AttackPower = new Observable<int>();
        public Observable<bool> Visibility = new Observable<bool>();
        public Observable<bool> ButtonVisibility = new Observable<bool>();

        public Action ResetFunction;
        public Action UnsafeResetFunction;
        private BattleConductor _conductor;
        private ActorData actorToManipulate;
    
        public DebugViewModel(Action resetFunction, Action unsafeResetFunction, ActorData actorData)
        {
            ResetFunction = resetFunction;
            UnsafeResetFunction = unsafeResetFunction;
            actorToManipulate = actorData;
            ButtonVisibility.Value = false;
            SetToActorData(actorToManipulate);
            SetVisibility(false);
        }
        
        public void SetButtonVisibility(bool visibility)
        {
            ButtonVisibility.Value = visibility;
        }

        public void SetToActorData(ActorData actorData)
        {
            SendToActorData(actorData);
            HealthValue.Value = actorToManipulate.Health;
            HealPower.Value = actorToManipulate.HealActionData.HealAmount;
            AttackPower.Value = actorToManipulate.AttackActionData.HPDamage;
        }
    
        private void SendToActorData(ActorData actorData)
        {
            actorToManipulate.Name = actorData.Name;
            actorToManipulate.Health = actorData.Health;
        
            CopyActionDataFromTo(actorData.HealActionData,actorToManipulate.HealActionData);
            CopyActionDataFromTo(actorData.AttackActionData, actorToManipulate.AttackActionData);
            CopyActionDataFromTo(actorData.GuardActionData, actorToManipulate.GuardActionData);
        
            actorToManipulate.Icon = actorData.Icon;
            actorToManipulate.HighResIcon = actorData.HighResIcon;
        }
        
        private void CopyActionDataFromTo(BattleActionData source, BattleActionData destination)
        {
            destination.HPDamage = source.HPDamage;
            destination.HealAmount = source.HealAmount;
            destination.DoesApplyGuard = source.DoesApplyGuard;
            destination.ActionName = source.ActionName;
            destination.BattleActionType = source.BattleActionType;
        }
        

        private void UpdateActorWithCurrentData()
        {
            actorToManipulate.Health = HealthValue;
            actorToManipulate.HealActionData.HealAmount = HealPower;
            actorToManipulate.AttackActionData.HPDamage = AttackPower;
        }

        public void TriggerResetWithData()
        {
            UpdateActorWithCurrentData();
            ResetFunction?.Invoke();
        }

        public void TriggerResetWithDataUnsafe()
        {
            UpdateActorWithCurrentData();
            UnsafeResetFunction?.Invoke();
        }
    
        public void OpenDebugMenu()
        {
            SetVisibility(true);
        }

        public void CloseDebugMenu()
        {
            SetVisibility(false);
        }
        public void SetVisibility(bool visibility)
        {
            Visibility.Value = visibility;
        }
    
        public void AddHealth()
        {
            HealthValue.Value++;
        }
    
        public void SubtractHealth()
        {
            HealthValue.Value = Math.Max(0, HealthValue.Value - 1);
        }
    
        public void AddHealPower()
        {
            HealPower.Value++;
        }
    
        public void SubtractHealPower()
        {
            HealPower.Value = Math.Max(0, HealPower.Value - 1);
        }
    
        public void AddAttackPower()
        {
            AttackPower.Value++;
        }
    
        public void SubtractAttackPower()
        {
            AttackPower.Value = Math.Max(0, AttackPower.Value - 1);
        }
    }
}
