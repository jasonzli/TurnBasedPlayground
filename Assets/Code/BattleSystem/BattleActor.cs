using Code.ScriptableObjects;

namespace Code.BattleSystem
{
    /// <summary>
    /// A base class for the battle actors. This is the data that the battle system will use to perform actions.
    /// Extension is possible here, but it is not necessary for the scope of this project.
    /// </summary>
    public class BattleActor : IBattleActor
    {
        private string _name;
        public string Name { get => _name; set => _name = value; }
        private int _maxHP;
        public int MaxHP { get => _maxHP; set => _maxHP = value; }
        private int _currentHP;
        public int CurrentHP { get => _currentHP; set => _currentHP = value; }
        private bool _guarded;
        public bool Guarded { get => _guarded; set => _guarded = value; }
        
        public BattleActor(string name, int maxHP, bool guarded)
        {
            Name = name;
            MaxHP = maxHP;
            CurrentHP = maxHP;
            Guarded = guarded;
        }

        public BattleActor(ActorData actorData)
        {
            Name = actorData.Name;
            MaxHP = actorData.Health;
            CurrentHP = actorData.Health;
            Guarded = false;
        }
    }
}