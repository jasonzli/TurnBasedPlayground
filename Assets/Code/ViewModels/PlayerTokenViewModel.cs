using System;
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
        
        public PlayerTokenViewModel(
            Transform cameraTransform, 
            Transform targetTransform)
        {
            CameraTransform = cameraTransform;
            TargetTransform = targetTransform;
        }
        
        public void LookAtTarget()
        {
            OnLookAtTarget?.Invoke();
        }
        
        public void LookAtCamera()
        {
            OnLookAtCamera?.Invoke();
        }
        
        public void SetVisibility(bool visibility)
        {
            Visibility.Value = visibility;
        }
    }
}