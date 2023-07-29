namespace Code.BattleSystem
{
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