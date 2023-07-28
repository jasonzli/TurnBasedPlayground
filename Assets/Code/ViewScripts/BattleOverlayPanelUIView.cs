
using Code.ViewModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.ViewScripts
{
    //This overlay panel can be used to show the winner of the battle or simply show phases of the battle (like Combat Begins
    public class BattleOverlayPanelUIView : ViewBase
    {

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
            SetVisibility(context.Visibility);
        }

        
        private void SetVisibility(bool visibility)
        {
            if (visibility)
            {
                Show();
            }
            else
            {
                Hide();
            }
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