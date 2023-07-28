using Code.BattleSystem;
using Code.ProtoVM;

namespace Code.ViewModels
{
    /// <summary>
    /// Player panel viewmodel for showing the panels on the left and right of the screen
    /// </summary>
    public class PlayerPanelViewModel : IViewModel
    {
        private IBattleActor _actorContext;
        public Observable<int> CurrentHP = new Observable<int>();
        public Observable<int> MaxHP = new Observable<int>();
        public Observable<bool> IsGuarding = new Observable<bool>();
        public Observable<string> Name = new Observable<string>();
        public Observable<bool> IsP1Side = new Observable<bool>();
        public Observable<bool> Visibility = new Observable<bool>();

        public PlayerPanelViewModel(IBattleActor actorContext, bool isP1Side)
        {
            _actorContext = actorContext;
            SetupFromBattleActor(_actorContext, isP1Side);
        }

        public void SetupFromBattleActor(IBattleActor actor, bool isP1Side)
        {
            CurrentHP.Value = actor.CurrentHP;
            MaxHP.Value = actor.MaxHP;
            IsGuarding.Value = actor.Guarded;
            Name.Value = actor.Name;
            IsP1Side.Value = isP1Side;
        }

        public void UpdateFromBattleActor()
        {
            CurrentHP.Value = _actorContext.CurrentHP;
            IsGuarding.Value = _actorContext.Guarded;
        }
        
        public void SetVisibility(bool visibility)
        {
            Visibility.Value = visibility;
        }
    }
}