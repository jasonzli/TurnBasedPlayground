namespace Code.BattleSystem
{
    /// <summary>
    /// "unsafe" Battle Actions are the same except you can do negative damage and healing
    /// This is used because it was fun to balance around this for the Elemental and TwinSnake
    /// </summary>
    /// <remarks>
    /// Another constraint on this was the way the enemy's move data is created, so I didn't really bother
    /// with creating a lot of different types of battle actions, but this serves as an example of how you could
    /// </remarks>
    public class UnsafeBattleAction : BattleActionBase
    {
        public UnsafeBattleAction(IBattleAction safeAction)
        {
            Parameters = safeAction.Parameters;
            Source = safeAction.Source;
            Target = safeAction.Target;
        }

        public override bool Execute()
        {
            //No checks, do whatever you want
            ApplyHPDamage(Target);
            ApplyHeal(Source);
            
            //If parameters guard, apply guard!
            if (Parameters.doesApplyGuard)
            {
                ApplyGuard(Source);
            }

            //if target is still guarded, remove the guard
            if (Target.Guarded)
            {
                Target.Guarded = false;
            }
            
            return true;
        }
    }
}