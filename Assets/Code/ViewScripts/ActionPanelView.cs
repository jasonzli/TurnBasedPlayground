using Code.ViewModels;
using TMPro;
using UnityEngine;

namespace Code.ViewScripts
{
    
    /// <summary>
    /// View for the action panel in the center of the screen
    /// Does some text updating for the body and header
    /// </summary>
    public class ActionPanelView : ViewBase
    {
        private ActionPanelViewModel _context;
        [SerializeField] private Animator _animator;
        [SerializeField] private TextMeshProUGUI _headerText;
        [SerializeField] private TextMeshProUGUI _bodyText;

        public void Initialize(ActionPanelViewModel context)
        {
            _context = context;
            gameObject.SetActive(true);
            
            _context.ActionName.PropertyChanged += UpdateBody;
            _context.SourceName.PropertyChanged += UpdateName;

            _context.Visibility.PropertyChanged += SetVisibility;
            
            ResetTriggers();
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
        private void ResetTriggers()
        {
            _animator.ResetTrigger("Show");
            _animator.ResetTrigger("Hide");
        }
        public override void Show()
        {
            ResetTriggers();
            _animator.SetTrigger("Show");
        }

        public override void Hide()
        {
            ResetTriggers();
            _animator.SetTrigger("Hide");
        }

    }
}