using System.Collections;
using System.Collections.Generic;
using Code.ViewModels;
using Code.ViewScripts;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(Animator),typeof(LookAtConstraint), typeof(PositionConstraint))]
public class PlayerTokenView : ViewBase
{
    [Header("This Animator should be the lower token animator")]
    [SerializeField] Animator _localTokenAnimator;

    [SerializeField]private LookAtConstraint _lookAtConstraint;
    [SerializeField]private PositionConstraint _positionConstraint;
    [SerializeField]private Animator _sceneMovementAnimator;
    
    private PlayerTokenViewModel _context;

    public void Initialize(PlayerTokenViewModel context)
    {

        _context = context;
        
        //remove all sources in lookAtConstraint
        
        //add sources to lookAtConstraint
        ConstraintSource cameraSource = new ConstraintSource();
        cameraSource.sourceTransform = _context.CameraTransform;
        ConstraintSource targetSource = new ConstraintSource();
        targetSource.sourceTransform = _context.TargetTransform;
        _lookAtConstraint.SetSource(0,cameraSource);
        _lookAtConstraint.SetSource(1,targetSource);
        _positionConstraint.SetSource(0,targetSource);

        _context.OnLookAtTarget += LookAtTarget;
        _context.OnLookAtCamera += LookAtCamera;

        ResetTriggers();
    }
    
    public void OverrideLocalAnimator(AnimatorOverrideController overrideController)
    {
        _localTokenAnimator.runtimeAnimatorController = overrideController;
    }
    
    public void OverrideMovementAnimator(AnimatorOverrideController overrideController)
    {
        _sceneMovementAnimator.runtimeAnimatorController = overrideController;
    }

    public void ResetTriggers()
    {
        _sceneMovementAnimator.ResetTrigger("LookAtTarget");
        _sceneMovementAnimator.ResetTrigger("LookAtCamera");
    }
    
    public void LookAtTarget()
    {
        ResetTriggers();
        _sceneMovementAnimator.SetTrigger("LookAtTarget");
    }
    
    public void LookAtCamera()
    {
        ResetTriggers();
        _sceneMovementAnimator.SetTrigger("LookAtCamera");
    }
}
