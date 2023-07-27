using Code.ScriptableObjects;

namespace Code.BattleSystem
{
    public abstract class BattleActorBase : IBattleActor
    {
        private string _name;
        public string Name { get => _name; set => _name = value; }
        private int _maxHP;
        public int MaxHP { get => _maxHP; set => _maxHP = value; }
        private int _currentHP;
        public int CurrentHP { get => _currentHP; set => _currentHP = value; }
        private bool _guarded;
        public bool Guarded { get => _guarded; set => _guarded = value; }
        
        protected BattleActorBase(string name, int maxHP, bool guarded)
        {
            Name = name;
            MaxHP = maxHP;
            CurrentHP = maxHP;
            Guarded = guarded;
        }

        protected BattleActorBase(ActorData actorData)
        {
            Name = actorData.Name;
            MaxHP = actorData.Health;
            CurrentHP = actorData.Health;
            Guarded = false;
        }

        protected BattleActorBase()
        {
            
        }
    }
}