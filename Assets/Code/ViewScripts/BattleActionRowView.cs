using System.Collections;
using System.Collections.Generic;
using Code.BattleSystem;
using Code.ScriptableObjects;
using Code.ViewScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A dumb container for the UI objects of the row
/// </summary>
public class BattleActionRowView : ViewBase
{
    public Button ActionButtonObject;
    public Image iconContainer;
    public TextMeshProUGUI ActionNameText;
    
    public void Initialize(BattleActionData actionData)
    {
        ActionNameText.text = actionData.ActionName;
        iconContainer.sprite = actionData.Icon;
    }

    public override void Show()
    {
        gameObject.SetActive(true);
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
    }
}
