using System;
using Code.ProtoVM;

namespace Code.ViewModels
{
    /// <summary>
    /// The overlay viewmodel
    ///
    /// This is more complex than it needed to be in this prototype. There was an intention to have it be *one* object
    /// that is updated as needed, but the way the prototype shook out, it was easier to just have 3 of them.
    /// </summary>
    public class BattleOverlayPanelViewModel : IViewModel
    {
        
        public Observable<string> HeaderText = new Observable<string>();
        public Observable<bool> ShowButton = new Observable<bool>();
        public Observable<bool> Visibility = new Observable<bool>();
        public Action PlayAgainButtonAction;
        
        
        public BattleOverlayPanelViewModel(
            string headerText,
            bool showButton, 
            bool visibility, 
            Action playAgainButtonAction)
        {
            HeaderText.Value = headerText;
            ShowButton.Value = showButton;
            Visibility.Value = visibility;
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

        public void SetVisibility(bool visibility)
        {
            Visibility.Value = visibility;
        }
    }
}