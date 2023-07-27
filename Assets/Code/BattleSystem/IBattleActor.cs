namespace Code.BattleSystem
{
    public interface IBattleActor
    {
        public string Name { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public bool Guarded { get; set; }
    }
}