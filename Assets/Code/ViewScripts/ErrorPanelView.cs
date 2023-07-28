using Code.ViewModels;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

namespace Code.ViewScripts
{
    /// <summary>
    /// View class for setting up the error panel
    /// </summary>
    public class ErrorPanelView : ViewBase
    {
        private ErrorPanelViewModel _context;
        [SerializeField] private TextMeshProUGUI _errorMessage;
        [SerializeField] private Button ResetButton;
        [SerializeField] private Button TryAgainButton;

        public void Initialize(ErrorPanelViewModel context)
        {
            _context = context;
            
            context.ErrorMessage.PropertyChanged += UpdateErrorString;
            context.Visibility.PropertyChanged += SetVisibility;
            ResetButton.onClick.RemoveAllListeners();
            TryAgainButton.onClick.RemoveAllListeners();
            
            ResetButton.onClick.AddListener(() => { context.ResetFunction();});
            TryAgainButton.onClick.AddListener(() => { context.TryAgainFunction();});
            
            //Set visibility automation
            ResetButton.onClick.AddListener(() =>
            {
                SetVisibility(false);
            });
            TryAgainButton.onClick.AddListener(() =>
            {
                SetVisibility(false);
            });
        }
        
        private void UpdateErrorString(string message)
        {
            _errorMessage.text = message;
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