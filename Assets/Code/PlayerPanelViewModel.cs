using Code.ProtoVM;

namespace Code
{
    public class PlayerPanelViewModel
    {
        public Observable<int> CurrentHP = new Observable<int>();
        public Observable<int> MaxHP = new Observable<int>();
        public Observable<bool> IsGuarding = new Observable<bool>();
        public Observable<string> Name = new Observable<string>();
        public Observable<bool> IsP1Side = new Observable<bool>();

        public PlayerPanelViewModel(IBattleActor actorContext, bool isP1Side)
        {
            SetupFromBattleActor(actorContext, isP1Side);
        }

        public void SetupFromBattleActor(IBattleActor actor, bool isP1Side)
        {
            CurrentHP.Value = actor.CurrentHP;
            MaxHP.Value = actor.MaxHP;
            IsGuarding.Value = actor.Guarded;
            Name.Value = actor.Name;
            IsP1Side.Value = isP1Side;
        }

        public void UpdateFromBattleActor(IBattleActor actor)
        {
            CurrentHP.Value = actor.CurrentHP;
            IsGuarding.Value = actor.Guarded;
        }
    }
}