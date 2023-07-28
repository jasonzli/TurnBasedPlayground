using System;
using Code.BattleSystem;
using Code.ProtoVM;
using Code.ScriptableObjects;
using Code.ViewModels;

namespace Code.DebugMenu
{
    public class DebugViewModel : IViewModel
    {

        public Observable<int> HealthValue = new Observable<int>();
        public Observable<int> HealPower = new Observable<int>();
        public Observable<int> AttackPower = new Observable<int>();
        public Observable<bool> Visibility = new Observable<bool>();

        public Action ResetFunction;
        private BattleConductor _conductor;
        private ActorData actorToManipulate;
    
        public DebugViewModel(Action resetFunction, ActorData actorData)
        {
            ResetFunction = resetFunction;
            actorToManipulate = actorData;
            SetToActorData(actorToManipulate);
            SetVisibility(false);
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
        
            actorToManipulate.HealActionData.HealAmount = actorData.HealActionData.HealAmount;
            actorToManipulate.HealActionData.ActionName = actorData.HealActionData.ActionName;
        
            actorToManipulate.AttackActionData.HPDamage = actorData.AttackActionData.HPDamage;
            actorToManipulate.AttackActionData.ActionName = actorData.AttackActionData.ActionName;
        
            actorToManipulate.GuardActionData.ActionName = actorData.GuardActionData.ActionName;
        
            actorToManipulate.Icon = actorData.Icon;
        }

        public void TriggerReset()
        {
            ResetFunction?.Invoke();
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
            HealthValue.Value--;
        }
    
        public void AddHealPower()
        {
            HealPower.Value++;
        }
    
        public void SubtractHealPower()
        {
            HealPower.Value--;
        }
    
        public void AddAttackPower()
        {
            AttackPower.Value++;
        }
    
        public void SubtractAttackPower()
        {
            AttackPower.Value--;
        }
    }
}
