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
        
        //Send all this to the lower controller
        _context.OnWounded += Wounded;
        _context.OnGuarded += Guarded;
        _context.OnAttack += Attack;
        _context.OnHeal += Heal;
        _context.OnKnockdown += Knockdown;

        ResetPivotTriggers();
        ResetLocalTriggers();
        _localTokenAnimator.Play("Idle");
    }
    
    public void OverrideLocalAnimator(AnimatorOverrideController overrideController)
    {
        _localTokenAnimator.runtimeAnimatorController = overrideController;
    }
    
    public void OverrideMovementAnimator(AnimatorOverrideController overrideController)
    {
        _sceneMovementAnimator.runtimeAnimatorController = overrideController;
    }

    public void ResetPivotTriggers()
    {
        _sceneMovementAnimator.ResetTrigger("LookAtTarget");
        _sceneMovementAnimator.ResetTrigger("LookAtCamera");
    }

    public void LookAtTarget()
    {
        ResetPivotTriggers();
        _sceneMovementAnimator.SetTrigger("LookAtTarget");
    }
    
    public void LookAtCamera()
    {
        ResetPivotTriggers();
        _sceneMovementAnimator.SetTrigger("LookAtCamera");
    }

    public void ResetLocalTriggers()
    {
        _localTokenAnimator.ResetTrigger("Wound");
        _localTokenAnimator.ResetTrigger("Guard");
        _localTokenAnimator.ResetTrigger("Attack");
        _localTokenAnimator.ResetTrigger("Heal");
        _localTokenAnimator.ResetTrigger("Knockdown");
    }
    
    public void Wounded()
    {
        ResetLocalTriggers();
        _localTokenAnimator.SetTrigger("Wound");
    }
    
    public void Guarded()
    {
        ResetLocalTriggers();
        _localTokenAnimator.SetTrigger("Guard");
    }
    
    public void Attack()
    {
        ResetLocalTriggers();
        _localTokenAnimator.SetTrigger("Attack");
    }
    
    public void Heal()
    {
        ResetLocalTriggers();
        _localTokenAnimator.SetTrigger("Heal");
    }
    
    public void Knockdown()
    {
        ResetLocalTriggers();
        _localTokenAnimator.SetTrigger("Knockdown");
    }
}
