using Code.ViewModels;
using TMPro;
using UnityEngine;

namespace Code.ViewScripts
{
    //View for the action panel in the center of the screen
    public class ActionPanelView : ViewBase
    {
        private ActionPanelViewModel _context;
        [SerializeField] private TextMeshProUGUI _headerText;
        [SerializeField] private TextMeshProUGUI _bodyText;

        public void Initialize(ActionPanelViewModel context)
        {
            _context = context;
            _context.ActionName.PropertyChanged += UpdateBody;
            _context.SourceName.PropertyChanged += UpdateName;

            _context.Visibility.PropertyChanged += SetVisibility;
            
            SetVisibility(_context.Visibility);
        }
        
        private void UpdateBody(string bodyText)
        {
            _bodyText.text = bodyText.ToUpper();
        }
        
        private void UpdateName(string nameText)
        {
            _headerText.text = nameText.ToUpper() + " USED";
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