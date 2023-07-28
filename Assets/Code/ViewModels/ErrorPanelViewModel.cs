using System;
using Code.ProtoVM;

namespace Code.ViewModels
{
    /// <summary>
    /// Viewmodel class for the ErrorPanel, has two functions that can be configured on setup
    /// </summary>
    public class ErrorPanelViewModel : IViewModel
    {
        public Observable<string> ErrorMessage = new Observable<string>();
        public Observable<bool> Visibility = new Observable<bool>();

        public Action ResetFunction;
        public Action TryAgainFunction;

        public ErrorPanelViewModel(Action resetFunction, Action tryAgainFunction)
        {
            Initialize(resetFunction, tryAgainFunction);
        }

        public void Initialize(Action resetFunction, Action tryAgainFunction)
        {
            Visibility.Value = false;
            ResetFunction = resetFunction;
            TryAgainFunction = tryAgainFunction;
        }

        public void UpdateErrorMessage(string message)
        {
            ErrorMessage.Value = message;
        }

        public void SetVisibility(bool visibility)
        {
            Visibility.Value = visibility;
        }
        
    }
}