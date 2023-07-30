using System;
using System.Collections.Generic;
using Code.BattleSystem;
using Code.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.ViewScripts
{
    /// <summary>
    /// A simple container for the UI objects of the row
    /// Visibility is completely controlled by its parent, the BattleActionSelectionPanel
    /// </summary>
    public class BattleActionRowView : ViewBase
    {
        [SerializeField] private Button ActionButtonObject;
        [SerializeField] private Image iconContainer;
        [SerializeField] private TextMeshProUGUI ActionNameText;
        [SerializeField] private EventTrigger TooltipEventTrigger;
        [SerializeField] private TextMeshProUGUI ToolTipText;

        [SerializeField] private Animator _animator;

        public void Initialize(BattleActionData actionData, BattleActionParameters actionParameters, Action onClickAction)
        {
            ActionNameText.text = actionData.ActionName.ToUpper();
            iconContainer.sprite = actionData.Icon;
            ToolTipText.text = CreateToolTipText(actionParameters);

            ResetTriggers();
            ActionButtonObject.onClick.RemoveAllListeners();
            ActionButtonObject.onClick.AddListener(
                () =>
                {
                    onClickAction();
                    ResetTriggers();
                    _animator.SetTrigger("HideTooltip");
                });
            
            //Set up tooltip
            TooltipEventTrigger.triggers[0].callback.RemoveAllListeners();
            TooltipEventTrigger.triggers[1].callback.RemoveAllListeners();
            
            TooltipEventTrigger.triggers[0].callback.AddListener((data) =>
            {
                ResetTriggers();
                _animator.SetTrigger("ShowTooltip");
            });
            TooltipEventTrigger.triggers[1].callback.AddListener((data) =>
            {
                ResetTriggers();
                _animator.SetTrigger("HideTooltip");
            });
        }

        private void ResetTriggers()
        {
            _animator.ResetTrigger("ShowTooltip");
            _animator.ResetTrigger("HideTooltip");
        }
        private string CreateToolTipText(BattleActionParameters actionParameters)
        {
            List<string> toolTipTexts = new List<string>();
            
            if (actionParameters.Damage != 0)
            {
                string damageText = "";
                if (actionParameters.Damage > 0)
                {
                    damageText += $"Deals {actionParameters.Damage} damage";
                }
                else
                {
                    damageText += $"Heals enemy {Math.Abs(actionParameters.Damage)} damage";
                }

                if (damageText != "")
                {
                    toolTipTexts.Add(damageText);
                }
            }
            if (actionParameters.HealAmount != 0)
            {
                string healText = "";
                if (actionParameters.HealAmount > 0)
                {
                    healText += $"Heals {actionParameters.HealAmount} HP";
                }
                else
                {
                    healText += $"Inflicts {Math.Abs(actionParameters.HealAmount)} self damage";
                }
                
                if (healText != "")
                {
                    toolTipTexts.Add(healText);
                }
            }
            
            if (actionParameters.ApplyGuard)
            {
                string guardText = "Guards for a turn";
                toolTipTexts.Add(guardText);
            }
            
            string toolTipString = "";
            for(int i = 0; i < toolTipTexts.Count; i++)
            {
                if (i != 0)
                {
                    toolTipString += "\n";
                }
                toolTipString += toolTipTexts[i];
            }

            return toolTipString;
        }
    }
}
