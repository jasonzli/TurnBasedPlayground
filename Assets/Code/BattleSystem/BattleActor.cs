namespace Code.BattleSystem
{
    /// <summary>
    /// Simple instance of the actor. Origianlly thought I might try to make the actors more complex, but time was against it.
    /// </summary>
    public class BattleActor : BattleActorBase
    {
        public BattleActor(string name, int health, bool guarded) : base(name, health, guarded){}
    }
}