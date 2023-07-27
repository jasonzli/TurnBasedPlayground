using System;
using Code.ProtoVM;

namespace Code.ViewModels
{
    public class BattleOverlayPanelViewModel
    {
        
        public Observable<string> HeaderText = new Observable<string>();
        public Observable<bool> ShowButton = new Observable<bool>();
        public Observable<bool> IsVisible = new Observable<bool>();
        public Action PlayAgainButtonAction;
        public Action OnPanelHidden;
        
        public BattleOverlayPanelViewModel(
            string headerText,
            bool showButton, 
            bool isVisible, 
            Action playAgainButtonAction)
        {
            HeaderText.Value = headerText;
            ShowButton.Value = showButton;
            IsVisible.Value = isVisible;
            PlayAgainButtonAction = playAgainButtonAction;
        }

        public void UpdateOverlay(string text, bool button)
        {
            HeaderText.Value = text;
            ShowButton.Value = button;
        }

        public void OnPlayButtonClicked()
        {
            PlayAgainButtonAction();
        }
    }
}