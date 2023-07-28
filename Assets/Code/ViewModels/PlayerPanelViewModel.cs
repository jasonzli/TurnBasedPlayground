using Code.BattleSystem;
using Code.ProtoVM;
using Code.ScriptableObjects;

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
        
        
        public ActorData ActorData; //I really don't like this but it's how we'll pass the data along for now

        public PlayerPanelViewModel(IBattleActor actorContext, ActorData actorData, bool isP1Side)
        {
            _actorContext = actorContext;
            ActorData = actorData;
            SetupFromBattleActor(_actorContext, actorData, isP1Side);
        }

        public void SetupFromBattleActor(IBattleActor actor, ActorData actorData,bool isP1Side)
        {
            CurrentHP.Value = actor.CurrentHP;
            MaxHP.Value = actor.MaxHP;
            IsGuarding.Value = actor.Guarded;
            Name.Value = actor.Name;
            IsP1Side.Value = isP1Side;
            ActorData = actorData;
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