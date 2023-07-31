using System;
using Code.BattleSystem;
using Code.ProtoVM;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.ViewModels
{
    public class PlayerTokenViewModel : IViewModel
    {
        
        public Transform CameraTransform;
        public Transform TargetTransform;
        public Observable<bool> Visibility = new Observable<bool>();

        public Action OnLookAtTarget;
        public Action OnLookAtCamera;
        public Action OnWounded;
        public Action OnGuarded;
        public Action OnAttack;
        public Action OnHeal;
        public Action OnKnockdown;
        
        public PlayerTokenViewModel(
            Transform cameraTransform, 
            Transform targetTransform)
        {
            CameraTransform = cameraTransform;
            TargetTransform = targetTransform;
        }
        
        public void PerformActionAnimation(BattleActionType actionType)
        {
            switch (actionType)
            {
                case BattleActionType.Attack:
                    Attack();
                    break;
                case BattleActionType.Guard:
                    Guarded();
                    break;
                case BattleActionType.Heal:
                    Heal();
                    break;
            }
            
        }

        public void LookAtTarget()
        {
            OnLookAtTarget?.Invoke();
        }
        
        public void LookAtCamera()
        {
            OnLookAtCamera?.Invoke();
        }
        
        public void Wounded()
        {
            OnWounded?.Invoke();
        }
        
        public void Guarded()
        {
            OnGuarded?.Invoke();
        }
        
        public void Attack()
        {
            OnAttack?.Invoke();
        }
        
        public void Heal()
        {
            OnHeal?.Invoke();
        }
        
        public void Knockdown()
        {
            OnKnockdown?.Invoke();
        }
        
        public void SetVisibility(bool visibility)
        {
            Visibility.Value = visibility;
        }
    }
}