using Code.ScriptableObjects;
using TMPro;
using UnityEngine.UI;

namespace Code.ViewScripts
{
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
            ActionNameText.text = actionData.ActionName.ToUpper();
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
}
