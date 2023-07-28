using Code.BattleSystem;
using Code.ProtoVM;

namespace Code.ViewModels
{
    /// <summary>
    /// Viewmodel for controlling the center action panel
    /// </summary>
    public class ActionPanelViewModel : IViewModel
    {
        public Observable<string> SourceName = new Observable<string>();
        public Observable<string> ActionName = new Observable<string>();
        public Observable<bool> Visibility = new Observable<bool>();
        
        public void SetAction(IBattleAction action)
        {
            SourceName.Value = action.Source.Name;
            ActionName.Value = action.Parameters.MoveName;
        }


        public void SetVisibility(bool visibility)
        {
            Visibility.Value = visibility;
        }
    }
}