using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// A container for the tooltip behavior. This is separate from a view *for now*
/// With other statuses, it would make sense to abstract this out and configurable
/// </summary>
public class StatusContentContainer : MonoBehaviour
{
    
    [SerializeField] EventTrigger _tooltipEventTrigger;
    [SerializeField] Animator _animator;

    void OnEnable()
    {
        _animator = GetComponent<Animator>();
        ResetTriggers();
        //Clear Event triggers
        //At this point, by convention, 0 is enter, 1 is exit
        _tooltipEventTrigger.triggers[0].callback.RemoveAllListeners();
        _tooltipEventTrigger.triggers[1].callback.RemoveAllListeners();
            
        //Set up animations
        _tooltipEventTrigger.triggers[0].callback.AddListener((data) =>
        {
            ResetTriggers();
            _animator.SetTrigger("Show");
        });
        _tooltipEventTrigger.triggers[1].callback.AddListener((data) =>
        {
            ResetTriggers();
            _animator.SetTrigger("Hide");
        });
    }
    
    private void ResetTriggers()
    {
        _animator.ResetTrigger("Show");
        _animator.ResetTrigger("Hide");
    }
        
    public void Show()
    {
        ResetTriggers();
        _animator.SetTrigger("Show");
    }
        
    public void Hide()
    {
        ResetTriggers();
        _animator.SetTrigger("Hide");
    }
}
