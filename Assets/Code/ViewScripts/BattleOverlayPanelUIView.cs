
using System.Collections;
using Code.ViewModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.ViewScripts
{
    //This overlay panel can be used to show the winner of the battle or simply show phases of the battle (like Combat Begins
    public class BattleOverlayPanelUIView : ViewBase
    {

        [SerializeField] private Animator _animator;
        [SerializeField] private TextMeshProUGUI _headerText;
        [SerializeField] private GameObject _buttonContainer;
        [SerializeField] private Button _buttonObject;
        [SerializeField] private bool _showButton;
        [SerializeField] private string _battleBeingsText = "Combat Begins";

        private BattleOverlayPanelViewModel _context;
        private Button _playAgainButton;
        
        public void Initialize(BattleOverlayPanelViewModel context)
        {
            _context = context;
            gameObject.SetActive(true);
            
            _headerText.text = context.HeaderText;
            _showButton = context.ShowButton;
            _buttonObject.onClick.RemoveAllListeners();
            _buttonObject.onClick.AddListener( () =>
                {
                    _context.OnPlayButtonClicked();
                }
            );
            SetButtonActive(_showButton);
            
            
            context.HeaderText.PropertyChanged += SetText;
            context.ShowButton.PropertyChanged += SetButtonActive;
            context.Visibility.PropertyChanged += SetVisibility;

            SetText(context.HeaderText);
            
            ResetTriggers();
            SetVisibility(context.Visibility);
        }

        public void SetButtonActive(bool buttonActive)
        {
            _showButton = buttonActive;
            _buttonContainer.SetActive(_showButton);
        }

        public void SetText(string text)
        {
            _headerText.text = text.ToUpper();
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