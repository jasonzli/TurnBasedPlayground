using Code.ScriptableObjects;
using TMPro;
using UnityEngine.UI;

namespace Code.ViewScripts
{
    /// <summary>
    /// A simple container for the UI objects of the row
    /// Visibility is completely controlled by its parent, the BattleActionSelectionPanel
    /// </summary>
    public class BattleActionRowView : ViewBase
    {
        public Button ActionButtonObject;
        public Image iconContainer;
        public TextMeshProUGUI ActionNameText;
    
        public void Initialize(BattleActionData actionData)
        {
            ActionNameText.text = actionData.ActionName.ToUpper();
            iconContainer.sprite = actionData.Icon;
        }
    }
}
