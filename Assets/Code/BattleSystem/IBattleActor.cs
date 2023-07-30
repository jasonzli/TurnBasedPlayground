namespace Code.BattleSystem
{
    /// <summary>
    /// Essential data for BattleActors
    /// Additional statuses could be added here
    /// </summary>
    public interface IBattleActor
    {
        public string Name { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public bool Guarded { get; set; }
    }
}